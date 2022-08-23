//
//  GameCenter.h
//  GameCenter
//
//  Created by Kashif Tasneem on 01/02/2014.
//  Copyright (c) 2014 Kashif Tasneem. All rights reserved.
//

#import <Foundation/Foundation.h>
#import <GameKit/GameKit.h>
#import "GameCenterManager.h"

@interface GameCenter : NSObject<GKLeaderboardViewControllerDelegate,GameCenterManagerDelegate,GKAchievementViewControllerDelegate> {
    BOOL gameCenterAvailable_;
    BOOL userAuthenticated_;
    GameCenterManager* gameCenterManager_;
    NSString *gameObjectName_;
}

@property (nonatomic,retain) NSString *playerAlias;
@property (nonatomic,assign) NSString *playerName;
@property (nonatomic,assign) NSString *playerId;

+ (GameCenter*) shareGameCenter;
- (void)authenticateLocalUserWithGameObject:(NSString*)gameObject;
- (BOOL)isGameCenterAvailable;
- (void)authenticationChanged;

-(void) showLeaderboard:(NSString*) leaderboardId;
-(void) showAchievements;
-(void) resetAchivements;
-(void) submitScore:(int) score ForLeaderboardId:(NSString*) leaderboardId;
-(void) submitFloatScore:(float) score WithDecimals:(int) decimals ForLeaderboardId:(NSString*) leaderboardId;
-(void) submitAchievment:(NSString*) achId WithPercantage:(int) percantage andShouldShowBanner:(BOOL) show;
-(void) submitIncrementalAchievement:(NSString*) achId WithPercantage:(float) percantage andShouldShowBanner:(BOOL) show;
-(void) getHighestScore:(NSString*) leaderboardId;
@end
