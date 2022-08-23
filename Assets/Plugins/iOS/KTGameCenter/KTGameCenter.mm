#import "GameCenter.h"

extern "C" {
    void authenticateLocalUser(const char* gameobjectName) {
        [[GameCenter shareGameCenter] authenticateLocalUserWithGameObject:[NSString stringWithUTF8String:gameobjectName]];
    }
    void showLeaderboard(const char* leaderboardId) {
        if (leaderboardId == NULL) {
            [[GameCenter shareGameCenter] showLeaderboard:nil];
        }
        else {
            [[GameCenter shareGameCenter] showLeaderboard:[NSString stringWithUTF8String:leaderboardId]];
        }
    }
    void resetAchievements() {
        [[GameCenter shareGameCenter] resetAchivements];
    }
    void submitScore(int score,const char* leaderboard) {
        [[GameCenter shareGameCenter] submitScore:score ForLeaderboardId:[NSString stringWithUTF8String:leaderboard]];
    }
    
    void submitFloatScore(float score,int decimals,const char* leaderboard) {
        [[GameCenter shareGameCenter] submitFloatScore:score WithDecimals:decimals ForLeaderboardId:[NSString stringWithUTF8String:leaderboard]];
    }
    
    void submitAchievement(int percantage,const char* achivement,BOOL showBanner) {
        [[GameCenter shareGameCenter] submitAchievment:[NSString stringWithUTF8String:achivement] WithPercantage:percantage andShouldShowBanner:showBanner];
    }
    
    void submitIncrementalAchievement(float percantage,const char* achivement,BOOL showBanner) {
        [[GameCenter shareGameCenter] submitIncrementalAchievement:[NSString stringWithUTF8String:achivement] WithPercantage:percantage andShouldShowBanner:showBanner];
    }
    
    void showAchievements () {
        [[GameCenter shareGameCenter] showAchievements];
    }
    void getMyScore(const char* leaderboardId) {
        [[GameCenter shareGameCenter] getHighestScore:[NSString stringWithUTF8String:leaderboardId]];
    }
}
