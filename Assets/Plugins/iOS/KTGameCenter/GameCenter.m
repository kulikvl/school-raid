//
//  GameCenter.m
//  GameCenter
//
//  Created by Kashif Tasneem on 01/02/2014.
//  Copyright (c) 2014 Kashif Tasneem. All rights reserved.
//

#import "GameCenter.h"

static GameCenter* shareObject = nil;

@implementation GameCenter

@synthesize playerAlias;
@synthesize playerName;
@synthesize playerId;

+(GameCenter*) shareGameCenter {
    if (shareObject == nil) {
        shareObject = [[GameCenter alloc] init];
    }
    return shareObject;
}
- (BOOL)isGameCenterAvailable {
    // check for presence of GKLocalPlayer API
    Class gcClass = (NSClassFromString(@"GKLocalPlayer"));
    // check if the device is running iOS 4.1 or later
    NSString *reqSysVer = @"4.1";
    NSString *currSysVer = [[UIDevice currentDevice] systemVersion];
    BOOL osVersionSupported = ([currSysVer compare:reqSysVer
                                           options:NSNumericSearch] != NSOrderedAscending);
    return (gcClass && osVersionSupported);
}

- (id)init {
    if ((self = [super init])) {
        gameCenterAvailable_ = [self isGameCenterAvailable];
        if (gameCenterAvailable_) {
            gameCenterManager_ = [[GameCenterManager alloc] init];
            [gameCenterManager_ setDelegate:self];
            
            NSNotificationCenter *nc =
            [NSNotificationCenter defaultCenter];
            [nc addObserver:self
                   selector:@selector(authenticationChanged)
                       name:GKPlayerAuthenticationDidChangeNotificationName
                     object:nil];
        }
    }
    return self;
}

- (void)authenticationChanged {
    if ([GKLocalPlayer localPlayer].isAuthenticated && !userAuthenticated_) {
        NSLog(@"Authentication changed: player authenticated.");
        userAuthenticated_ = YES;
        
    } else if (![GKLocalPlayer localPlayer].isAuthenticated && userAuthenticated_) {
        NSLog(@"Authentication changed: player not authenticated");
        userAuthenticated_ = NO;
    }
}
- (void)authenticateLocalUserWithGameObject:(NSString *)gameObject {
    
    if (gameObjectName_ != nil) {
        [gameObjectName_ release];
        gameObjectName_ = nil;
    }
    gameObjectName_ = [gameObject retain];
    if (!gameCenterAvailable_) {
        UnitySendMessage([gameObjectName_ UTF8String], "GameCenterAvailable", "false");
        return;
    }
    
    if ([GKLocalPlayer localPlayer].authenticated == NO) {
        [[GKLocalPlayer localPlayer] authenticateWithCompletionHandler:^(NSError *error) {
            NSString *finalString = @"";
            if (error != nil) {
                finalString = [error localizedDescription];
            }
            else {
                playerAlias = [[GKLocalPlayer localPlayer] alias];
                playerName = [[GKLocalPlayer localPlayer] displayName];
                playerId = [[GKLocalPlayer localPlayer] playerID];
            }
            
            if (playerAlias == nil) {
                playerAlias = @"";
            }
            if (playerId == nil) {
                playerId = @"";
            }
            if (playerName == nil) {
                playerName = @"";
            }
            NSString *variables = [NSString stringWithFormat:@"%@_%@_%@",playerAlias,playerName,playerId];
            UnitySendMessage([gameObjectName_ UTF8String], "SetVariables", [variables UTF8String]);
            
            UnitySendMessage([gameObjectName_ UTF8String], "IsAuthenticated", [finalString UTF8String]);
        }];
        userAuthenticated_ = NO;
    } else {
        NSLog(@"Already authenticated!");
        userAuthenticated_ = YES;
    }
}

-(void) showLeaderboard:(NSString*) leaderboardId
{
	GKLeaderboardViewController *leaderboardController = [[GKLeaderboardViewController alloc] init];
	if (leaderboardController != NULL)
	{
		leaderboardController.category = leaderboardId;
		leaderboardController.timeScope = GKLeaderboardTimeScopeWeek;
		leaderboardController.leaderboardDelegate = self;
        [[[[UIApplication sharedApplication] keyWindow] rootViewController] presentViewController:leaderboardController animated:YES completion:nil];
	}
    [leaderboardController autorelease];
}

-(void) showAchievements
{
    GKAchievementViewController *achivementController = [[GKAchievementViewController alloc] init];
    if (achivementController != NULL) {
        achivementController.achievementDelegate = self;
        [[[[UIApplication sharedApplication] keyWindow] rootViewController] presentViewController:achivementController animated:YES completion:nil];
    }
}

-(void) submitScore:(int) score ForLeaderboardId:(NSString*) leaderboardId
{
    if (userAuthenticated_) {
        [gameCenterManager_ reportScore:score forCategory:leaderboardId];
    }
    else {
        NSString *finalString = [NSString stringWithFormat:@"%@_%@",leaderboardId,@"User Not Authenticated"];
        UnitySendMessage([gameObjectName_ UTF8String], "ScoreSubmitted",[finalString UTF8String]);
    }
}

-(void) submitFloatScore:(float) score WithDecimals:(int) decimals ForLeaderboardId:(NSString*) leaderboardId
{
    if (userAuthenticated_) {
        int mult = 1;
        while (decimals > 0) {
            mult *= 10;
            decimals--;
        }
        int64_t gcScore = (int64_t)(score * mult);
        
        [gameCenterManager_ reportScore:gcScore forCategory:leaderboardId];
    }
    else {
        NSString *finalString = [NSString stringWithFormat:@"%@_%@",leaderboardId,@"User Not Authenticated"];
        UnitySendMessage([gameObjectName_ UTF8String], "ScoreSubmitted",[finalString UTF8String]);
    }
}

-(void) submitAchievment:(NSString*) achId WithPercantage:(int) percantage andShouldShowBanner:(BOOL) show
{
    if (userAuthenticated_) {
        [gameCenterManager_ submitAchievement:achId percentComplete:percantage ShouldShowCompletionBanner:show IsIncremental:NO];
    }
    else {
        NSString *finalString = [NSString stringWithFormat:@"%@_%@",achId,@"User not Authenticated"];
        UnitySendMessage([gameObjectName_ UTF8String], "AchievementSubmitted",[finalString UTF8String]);
    }
}

-(void) submitIncrementalAchievement:(NSString*) achId WithPercantage:(float) percantage andShouldShowBanner:(BOOL) show
{
    if (userAuthenticated_) {
        [gameCenterManager_ submitAchievement:achId percentComplete:percantage ShouldShowCompletionBanner:show IsIncremental:YES];
    }
    else {
        NSString *finalString = [NSString stringWithFormat:@"%@_%@",achId,@"User not Authenticated"];
        UnitySendMessage([gameObjectName_ UTF8String], "AchievementSubmitted",[finalString UTF8String]);
    }
}

-(void) resetAchivements
{
    if (userAuthenticated_) {
        [gameCenterManager_ resetAchievements];
    }
    else {
        UnitySendMessage([gameObjectName_ UTF8String], "AchievementReset",[[NSString stringWithFormat:@"User Not Authenticated"] UTF8String]);
    }
}

- (void) processGameCenterAuth: (NSError*) error
{
    NSString *finalString = @"";
    if (error != nil) {
        finalString = [error localizedDescription];
    }
    UnitySendMessage([gameObjectName_ UTF8String], "ProcessGC", [finalString UTF8String]);
}

- (void) reloadScoresComplete: (NSString*) leaderBoardId error: (NSError*) error
{
    NSString *resultString = @"";
    if (error != nil) {
        resultString = [error localizedDescription];
    }
    NSString *finalString = [NSString stringWithFormat:@"%@_%@",leaderBoardId,resultString];
    UnitySendMessage([gameObjectName_ UTF8String], "ReloadScoresCompleted",[finalString UTF8String]);
}

- (void) scoreReported:(NSString *)leaderboardId error:(NSError *)error {
    NSString *resultString = @"";
    if (error != nil) {
        resultString = [error localizedDescription];
    }
    NSString *finalString = [NSString stringWithFormat:@"%@_%@",leaderboardId,resultString];
    UnitySendMessage([gameObjectName_ UTF8String], "ScoreSubmitted",[finalString UTF8String]);
}

- (void)leaderboardViewControllerDidFinish:(GKLeaderboardViewController *)viewController
{
    [[[[UIApplication sharedApplication] keyWindow] rootViewController] dismissViewControllerAnimated:YES completion:nil];
    
}

- (void)achievementViewControllerDidFinish:(GKAchievementViewController *)viewController
{
    [[[[UIApplication sharedApplication] keyWindow] rootViewController] dismissViewControllerAnimated:YES completion:nil];
}

- (void) achievementSubmitted: (NSString*) achievementId error:(NSError*) error
{
    NSString *resultString = @"";
    if (error != nil) {
        resultString = [error localizedDescription];
    }
    
    NSString *finalString = [NSString stringWithFormat:@"%@_%@",achievementId,resultString];
    UnitySendMessage([gameObjectName_ UTF8String], "AchievementSubmitted",[finalString UTF8String]);
}

- (void) achievementResetResult: (NSError*) error
{
    NSString *resultString = @"";
    if (error != nil) {
        resultString = [error localizedDescription];
    }
    UnitySendMessage([gameObjectName_ UTF8String], "AchievementReset",[resultString UTF8String]);
}

- (void) mappedPlayerIDToPlayer: (GKPlayer*) player error: (NSError*) error
{
    
}

-(void) getHighestScore:(NSString*) leaderboardId
{
    if (userAuthenticated_) {
        GKLeaderboard *leaderboardRequest = [[GKLeaderboard alloc] init];
        [leaderboardRequest setIdentifier:leaderboardId];
        [leaderboardRequest loadScoresWithCompletionHandler:^(NSArray *scores, NSError *error) {
            int64_t score = -1;
            NSString *errorString = @"";
            if (error) {
                NSLog(@"error fetching score %@", error);
                errorString = [error localizedDescription];
            }
            else if (scores) {
                GKScore *localPlayerScore = leaderboardRequest.localPlayerScore;
                score = [localPlayerScore value];
            }
            
            
            NSString *scoreString = [NSString stringWithFormat:@"%lld",score];
            NSString *finalString = [NSString stringWithFormat:@"%@_%@_%@",leaderboardId,scoreString,errorString];
            UnitySendMessage([gameObjectName_ UTF8String], "ScoreFetched",[finalString UTF8String]);
            
        }];
    }
}


-(void) dealloc
{
    if (gameObjectName_ != nil) {
        [gameObjectName_ release];
        gameObjectName_ = nil;
    }
    if (shareObject != nil) {
        [shareObject release];
        shareObject = nil;
    }
    [super dealloc];
}

@end
