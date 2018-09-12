//
// StartTimerIntent.m
//
// This file was automatically generated and should not be edited.
//

#import "StartTimerIntent.h"

@implementation StartTimerIntent

@dynamic workspace;

@end

@interface StartTimerIntentResponse ()

@property (readwrite, NS_NONATOMIC_IOSONLY) StartTimerIntentResponseCode code;

@end

@implementation StartTimerIntentResponse

@synthesize code = _code;

- (instancetype)initWithCode:(StartTimerIntentResponseCode)code userActivity:(nullable NSUserActivity *)userActivity {
    self = [super init];
    if (self) {
        _code = code;
        self.userActivity = userActivity;
    }
    return self;
}

@end
