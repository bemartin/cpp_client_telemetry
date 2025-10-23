using System;
using Foundation;
using ObjCRuntime;

namespace Microsoft.Applications.Events
{
	// @interface ODWCommonDataContext : NSObject
	[BaseType (typeof(NSObject), Name = "ODWCommonDataContext")]
	[Protocol]
	public interface CommonDataContext
	{
		// @property (readwrite, copy, nonatomic) NSString * _Nonnull DomainName;
		[Export ("DomainName")]
		string DomainName { get; set; }

		// @property (readwrite, copy, nonatomic) NSString * _Nonnull MachineName;
		[Export ("MachineName")]
		string MachineName { get; set; }

		// @property (readwrite, copy, nonatomic) NSMutableArray * _Nonnull UserNames;
		[Export ("UserNames", ArgumentSemantic.Copy)]
		NSMutableArray UserNames { get; set; }

		// @property (readwrite, copy, nonatomic) NSMutableArray * _Nonnull UserAliases;
		[Export ("UserAliases", ArgumentSemantic.Copy)]
		NSMutableArray UserAliases { get; set; }

		// @property (readwrite, copy, nonatomic) NSMutableArray * _Nonnull IpAddresses;
		[Export ("IpAddresses", ArgumentSemantic.Copy)]
		NSMutableArray IpAddresses { get; set; }

		// @property (readwrite, copy, nonatomic) NSMutableArray * _Nonnull LanguageIdentifiers;
		[Export ("LanguageIdentifiers", ArgumentSemantic.Copy)]
		NSMutableArray LanguageIdentifiers { get; set; }

		// @property (readwrite, copy, nonatomic) NSMutableArray * _Nonnull MachineIds;
		[Export ("MachineIds", ArgumentSemantic.Copy)]
		NSMutableArray MachineIds { get; set; }

		// @property (readwrite, copy, nonatomic) NSMutableArray * _Nonnull OutOfScopeIdentifiers;
		[Export ("OutOfScopeIdentifiers", ArgumentSemantic.Copy)]
		NSMutableArray OutOfScopeIdentifiers { get; set; }
	}

	// @interface ODWDiagnosticDataViewer : NSObject
	[BaseType (typeof(NSObject), Name = "ODWDiagnosticDataViewer")]
	[Protocol]
	public interface DiagnosticDataViewer
	{
		// +(void)initializeViewerWithMachineIdentifier:(NSString * _Nonnull)machineIdentifier;
		[Static]
		[Export ("initializeViewerWithMachineIdentifier:")]
		void InitializeViewerWithMachineIdentifier (string machineIdentifier);

		// +(void)enableRemoteViewer:(NSString * _Nonnull)endpoint completionWithResult:(void (^ _Nonnull)(_Bool))completion;
		[Static]
		[Export ("enableRemoteViewer:completionWithResult:")]
		void EnableRemoteViewer (string endpoint, Action<bool> completion);

		// +(_Bool)enableRemoteViewer:(NSString * _Nonnull)endpoint;
		[Static]
		[Export ("enableRemoteViewer:")]
		bool EnableRemoteViewer (string endpoint);

		// +(void)disableViewer:(void (^ _Nonnull)(_Bool))completion;
		[Static]
		[Export ("disableViewer:")]
		void DisableViewer (Action<bool> completion);

		// +(_Bool)viewerEnabled;
		[Static]
		[Export ("viewerEnabled")]
		bool ViewerEnabled { get; }

		// +(NSString * _Nullable)currentEndpoint;
		[Static]
		[NullAllowed, Export ("currentEndpoint")]
		string CurrentEndpoint { get; }

		// +(void)registerOnDisableNotification:(void (^ _Nonnull)(void))callback;
		[Static]
		[Export ("registerOnDisableNotification:")]
		void RegisterOnDisableNotification (Action callback);
	}

	// @interface ODWEventProperties : NSObject
	[BaseType (typeof(NSObject), Name = "ODWEventProperties")]
	[DisableDefaultCtor]
	public interface EventProperties
	{
		// @property (readwrite, copy, nonatomic) NSString * _Nonnull name;
		[Export ("name")]
		string Name { get; set; }

		// @property (readwrite, nonatomic) ODWEventPriority priority;
		[Export ("priority", ArgumentSemantic.Assign)]
		EventPriority Priority { get; set; }

		// @property (readonly, copy, nonatomic) NSDictionary<NSString *,id> * _Nonnull properties;
		[Export ("properties", ArgumentSemantic.Copy)]
		NSDictionary<NSString, NSObject> Properties { get; }

		// @property (readonly, copy, nonatomic) NSDictionary<NSString *,NSNumber *> * _Nonnull piiTags;
		[Export ("piiTags", ArgumentSemantic.Copy)]
		NSDictionary<NSString, NSNumber> PiiTags { get; }

		// @property (readwrite, nonatomic) NSString * _Nonnull eventType;
		[Export ("eventType")]
		string EventType { get; set; }

		// -(instancetype _Nonnull)initWithName:(NSString * _Nonnull)name;
		[Export ("initWithName:")]
		NativeHandle Constructor (string name);

		// -(instancetype _Nonnull)initWithName:(NSString * _Nonnull)name properties:(NSDictionary<NSString *,id> * _Nonnull)properties;
		[Export ("initWithName:properties:")]
		NativeHandle Constructor (string name, NSDictionary<NSString, NSObject> properties);

		// -(instancetype _Nonnull)initWithName:(NSString * _Nonnull)name properties:(NSDictionary<NSString *,id> * _Nonnull)properties piiTags:(NSDictionary<NSString *,NSNumber *> * _Nonnull)piiTags __attribute__((objc_designated_initializer));
		[Export ("initWithName:properties:piiTags:")]
		[DesignatedInitializer]
		NativeHandle Constructor (string name, NSDictionary<NSString, NSObject> properties, NSDictionary<NSString, NSNumber> piiTags);

		// -(void)setProperty:(NSString * _Nonnull)name withValue:(id _Nonnull)value;
		[Export ("setProperty:withValue:")]
		void SetProperty (string name, string value);

		// -(void)setProperty:(NSString * _Nonnull)name withValue:(id _Nonnull)value withPiiKind:(ODWPiiKind)piiKind;
		[Export ("setProperty:withValue:withPiiKind:")]
		void SetProperty (string name, string value, PiiKind piiKind);

		// -(void)setProperty:(NSString * _Nonnull)name withDoubleValue:(double)value;
		[Export ("setProperty:withDoubleValue:")]
		void SetProperty (string name, double value);

		// -(void)setProperty:(NSString * _Nonnull)name withDoubleValue:(double)value withPiiKind:(ODWPiiKind)piiKind;
		[Export ("setProperty:withDoubleValue:withPiiKind:")]
		void SetProperty (string name, double value, PiiKind piiKind);

		// -(void)setProperty:(NSString * _Nonnull)name withInt64Value:(int64_t)value;
		[Export ("setProperty:withInt64Value:")]
		void SetProperty (string name, long value);

		// -(void)setProperty:(NSString * _Nonnull)name withInt64Value:(int64_t)value withPiiKind:(ODWPiiKind)piiKind;
		[Export ("setProperty:withInt64Value:withPiiKind:")]
		void SetProperty (string name, long value, PiiKind piiKind);

		// -(void)setProperty:(NSString * _Nonnull)name withUInt8Value:(uint8_t)value;
		[Export ("setProperty:withUInt8Value:")]
		void SetProperty (string name, byte value);

		// -(void)setProperty:(NSString * _Nonnull)name withUInt8Value:(uint8_t)value withPiiKind:(ODWPiiKind)piiKind;
		[Export ("setProperty:withUInt8Value:withPiiKind:")]
		void SetProperty (string name, byte value, PiiKind piiKind);

		// -(void)setProperty:(NSString * _Nonnull)name withUInt64Value:(uint64_t)value;
		[Export ("setProperty:withUInt64Value:")]
		void SetProperty (string name, ulong value);

		// -(void)setProperty:(NSString * _Nonnull)name withUInt64Value:(uint64_t)value withPiiKind:(ODWPiiKind)piiKind;
		[Export ("setProperty:withUInt64Value:withPiiKind:")]
		void SetProperty (string name, ulong value, PiiKind piiKind);

		// -(void)setProperty:(NSString * _Nonnull)name withBoolValue:(BOOL)value;
		[Export ("setProperty:withBoolValue:")]
		void SetProperty (string name, bool value);

		// -(void)setProperty:(NSString * _Nonnull)name withBoolValue:(BOOL)value withPiiKind:(ODWPiiKind)piiKind;
		[Export ("setProperty:withBoolValue:withPiiKind:")]
		void SetProperty (string name, bool value, PiiKind piiKind);

		// -(void)setProperty:(NSString * _Nonnull)name withUUIDValue:(NSUUID * _Nonnull)value;
		[Export ("setProperty:withUUIDValue:")]
		void SetProperty (string name, NSUuid value);

		// -(void)setProperty:(NSString * _Nonnull)name withUUIDValue:(NSUUID * _Nonnull)value withPiiKind:(ODWPiiKind)piiKind;
		[Export ("setProperty:withUUIDValue:withPiiKind:")]
		void SetProperty (string name, NSUuid value, PiiKind piiKind);

		// -(void)setProperty:(NSString * _Nonnull)name withDateValue:(NSDate * _Nonnull)value;
		[Export ("setProperty:withDateValue:")]
		void SetProperty (string name, NSDate value);

		// -(void)setPrivacyMetadata:(ODWPrivacyDataType)privTags withODWDiagLevel:(ODWDiagLevel)privLevel;
		[Export ("setPrivacyMetadata:withODWDiagLevel:")]
		void SetPrivacyMetadata (PrivacyDataType privTags, DiagLevel privLevel);

		// -(void)setProperty:(NSString * _Nonnull)name withDateValue:(NSDate * _Nonnull)value withPiiKind:(ODWPiiKind)piiKind;
		[Export ("setProperty:withDateValue:withPiiKind:")]
		void SetProperty (string name, NSDate value, PiiKind piiKind);

		// -(void)setType:(NSString * _Nonnull)type;
		[Export ("setType:")]
		void SetType (string type);
	}

	[Static]
	partial interface Constants
	{
		// extern NSString *const _Nonnull ODWCFG_BOOL_ENABLE_ANALYTICS;
		[Field ("ODWCFG_BOOL_ENABLE_ANALYTICS", "__Internal")]
		NSString ODWCFG_BOOL_ENABLE_ANALYTICS { get; }

		// extern NSString *const _Nonnull ODWCFG_BOOL_ENABLE_MULTITENANT;
		[Field ("ODWCFG_BOOL_ENABLE_MULTITENANT", "__Internal")]
		NSString ODWCFG_BOOL_ENABLE_MULTITENANT { get; }

		// extern NSString *const _Nonnull ODWCFG_BOOL_ENABLE_CRC32;
		[Field ("ODWCFG_BOOL_ENABLE_CRC32", "__Internal")]
		NSString ODWCFG_BOOL_ENABLE_CRC32 { get; }

		// extern NSString *const _Nonnull ODWCFG_BOOL_ENABLE_HMAC;
		[Field ("ODWCFG_BOOL_ENABLE_HMAC", "__Internal")]
		NSString ODWCFG_BOOL_ENABLE_HMAC { get; }

		// extern NSString *const _Nonnull ODWCFG_BOOL_ENABLE_DB_DROP_IF_FULL;
		[Field ("ODWCFG_BOOL_ENABLE_DB_DROP_IF_FULL", "__Internal")]
		NSString ODWCFG_BOOL_ENABLE_DB_DROP_IF_FULL { get; }

		// extern NSString *const _Nonnull ODWCFG_BOOL_ENABLE_DB_COMPRESS;
		[Field ("ODWCFG_BOOL_ENABLE_DB_COMPRESS", "__Internal")]
		NSString ODWCFG_BOOL_ENABLE_DB_COMPRESS { get; }

		// extern NSString *const _Nonnull ODWCFG_BOOL_ENABLE_WAL_JOURNAL;
		[Field ("ODWCFG_BOOL_ENABLE_WAL_JOURNAL", "__Internal")]
		NSString ODWCFG_BOOL_ENABLE_WAL_JOURNAL { get; }

		// extern NSString *const _Nonnull ODWCFG_BOOL_ENABLE_NET_DETECT;
		[Field ("ODWCFG_BOOL_ENABLE_NET_DETECT", "__Internal")]
		NSString ODWCFG_BOOL_ENABLE_NET_DETECT { get; }

		// extern NSString *const _Nonnull ODWCFG_STR_COLLECTOR_URL;
		[Field ("ODWCFG_STR_COLLECTOR_URL", "__Internal")]
		NSString ODWCFG_STR_COLLECTOR_URL { get; }

		// extern NSString *const _Nonnull ODWCFG_STR_CACHE_FILE_PATH;
		[Field ("ODWCFG_STR_CACHE_FILE_PATH", "__Internal")]
		NSString ODWCFG_STR_CACHE_FILE_PATH { get; }

		// extern NSString *const _Nonnull ODWCFG_INT_CACHE_FILE_SIZE;
		[Field ("ODWCFG_INT_CACHE_FILE_SIZE", "__Internal")]
		NSString ODWCFG_INT_CACHE_FILE_SIZE { get; }

		// extern NSString *const _Nonnull ODWCFG_INT_RAM_QUEUE_SIZE;
		[Field ("ODWCFG_INT_RAM_QUEUE_SIZE", "__Internal")]
		NSString ODWCFG_INT_RAM_QUEUE_SIZE { get; }

		// extern NSString *const _Nonnull ODWCFG_INT_RAM_QUEUE_BUFFERS;
		[Field ("ODWCFG_INT_RAM_QUEUE_BUFFERS", "__Internal")]
		NSString ODWCFG_INT_RAM_QUEUE_BUFFERS { get; }

		// extern NSString *const _Nonnull ODWCFG_INT_TRACE_LEVEL_MASK;
		[Field ("ODWCFG_INT_TRACE_LEVEL_MASK", "__Internal")]
		NSString ODWCFG_INT_TRACE_LEVEL_MASK { get; }

		// extern NSString *const _Nonnull ODWCFG_INT_TRACE_LEVEL_MIN;
		[Field ("ODWCFG_INT_TRACE_LEVEL_MIN", "__Internal")]
		NSString ODWCFG_INT_TRACE_LEVEL_MIN { get; }

		// extern NSString *const _Nonnull ODWCFG_BOOL_ENABLE_TRACE;
		[Field ("ODWCFG_BOOL_ENABLE_TRACE", "__Internal")]
		NSString ODWCFG_BOOL_ENABLE_TRACE { get; }

		// extern NSString *const _Nonnull ODWCFG_STR_TRACE_FOLDER_PATH;
		[Field ("ODWCFG_STR_TRACE_FOLDER_PATH", "__Internal")]
		NSString ODWCFG_STR_TRACE_FOLDER_PATH { get; }

		// extern NSString *const _Nonnull ODWCFG_INT_SDK_MODE;
		[Field ("ODWCFG_INT_SDK_MODE", "__Internal")]
		NSString ODWCFG_INT_SDK_MODE { get; }

		// extern NSString *const _Nonnull ODWCFG_STR_PROVIDER_GROUP_ID;
		[Field ("ODWCFG_STR_PROVIDER_GROUP_ID", "__Internal")]
		NSString ODWCFG_STR_PROVIDER_GROUP_ID { get; }

		// extern NSString *const _Nonnull ODWCFG_INT_MAX_TEARDOWN_TIME;
		[Field ("ODWCFG_INT_MAX_TEARDOWN_TIME", "__Internal")]
		NSString ODWCFG_INT_MAX_TEARDOWN_TIME { get; }

		// extern NSString *const _Nonnull ODWCFG_INT_MAX_PENDING_REQ;
		[Field ("ODWCFG_INT_MAX_PENDING_REQ", "__Internal")]
		NSString ODWCFG_INT_MAX_PENDING_REQ { get; }

		// extern NSString *const _Nonnull ODWCFG_INT_MAX_PKG_DROP_ON_FULL;
		[Field ("ODWCFG_INT_MAX_PKG_DROP_ON_FULL", "__Internal")]
		NSString ODWCFG_INT_MAX_PKG_DROP_ON_FULL { get; }

		// extern NSString *const _Nonnull ODWCFG_INT_STORAGE_FULL_PCT;
		[Field ("ODWCFG_INT_STORAGE_FULL_PCT", "__Internal")]
		NSString ODWCFG_INT_STORAGE_FULL_PCT { get; }

		// extern NSString *const _Nonnull ODWCFG_INT_STORAGE_FULL_CHECK_TIME;
		[Field ("ODWCFG_INT_STORAGE_FULL_CHECK_TIME", "__Internal")]
		NSString ODWCFG_INT_STORAGE_FULL_CHECK_TIME { get; }

		// extern NSString *const _Nonnull ODWCFG_INT_RAMCACHE_FULL_PCT;
		[Field ("ODWCFG_INT_RAMCACHE_FULL_PCT", "__Internal")]
		NSString ODWCFG_INT_RAMCACHE_FULL_PCT { get; }

		// extern NSString *const _Nonnull ODWCFG_STR_PRAGMA_JOURNAL_MODE;
		[Field ("ODWCFG_STR_PRAGMA_JOURNAL_MODE", "__Internal")]
		NSString ODWCFG_STR_PRAGMA_JOURNAL_MODE { get; }

		// extern NSString *const _Nonnull ODWCFG_STR_PRAGMA_SYNCHRONOUS;
		[Field ("ODWCFG_STR_PRAGMA_SYNCHRONOUS", "__Internal")]
		NSString ODWCFG_STR_PRAGMA_SYNCHRONOUS { get; }

		// extern NSString *const _Nonnull ODWCFG_STR_PRIMARY_TOKEN;
		[Field ("ODWCFG_STR_PRIMARY_TOKEN", "__Internal")]
		NSString ODWCFG_STR_PRIMARY_TOKEN { get; }

		// extern NSString *const _Nonnull ODWCFG_STR_START_PROFILE_NAME;
		[Field ("ODWCFG_STR_START_PROFILE_NAME", "__Internal")]
		NSString ODWCFG_STR_START_PROFILE_NAME { get; }

		// extern NSString *const _Nonnull ODWCFG_STR_TRANSMIT_PROFILES;
		[Field ("ODWCFG_STR_TRANSMIT_PROFILES", "__Internal")]
		NSString ODWCFG_STR_TRANSMIT_PROFILES { get; }

		// extern NSString *const _Nonnull ODWCFG_MODULE_HTTP_CLIENT;
		[Field ("ODWCFG_MODULE_HTTP_CLIENT", "__Internal")]
		NSString ODWCFG_MODULE_HTTP_CLIENT { get; }

		// extern NSString *const _Nonnull ODWCFG_MODULE_TASK_DISPATCHER;
		[Field ("ODWCFG_MODULE_TASK_DISPATCHER", "__Internal")]
		NSString ODWCFG_MODULE_TASK_DISPATCHER { get; }

		// extern NSString *const _Nonnull ODWCFG_MODULE_DATA_VIEWER;
		[Field ("ODWCFG_MODULE_DATA_VIEWER", "__Internal")]
		NSString ODWCFG_MODULE_DATA_VIEWER { get; }

		// extern NSString *const _Nonnull ODWCFG_MODULE_DECORATOR;
		[Field ("ODWCFG_MODULE_DECORATOR", "__Internal")]
		NSString ODWCFG_MODULE_DECORATOR { get; }

		// extern NSString *const _Nonnull ODWCFG_MODULE_PRIVACY_GUARD;
		[Field ("ODWCFG_MODULE_PRIVACY_GUARD", "__Internal")]
		NSString ODWCFG_MODULE_PRIVACY_GUARD { get; }

		// extern NSString *const _Nonnull ODWCFG_MODULE_OFFLINE_STORAGE;
		[Field ("ODWCFG_MODULE_OFFLINE_STORAGE", "__Internal")]
		NSString ODWCFG_MODULE_OFFLINE_STORAGE { get; }

		// extern NSString *const _Nonnull ODWCFG_STR_FACTORY_NAME;
		[Field ("ODWCFG_STR_FACTORY_NAME", "__Internal")]
		NSString ODWCFG_STR_FACTORY_NAME { get; }

		// extern NSString *const _Nonnull ODWCFG_MAP_FACTORY_CONFIG;
		[Field ("ODWCFG_MAP_FACTORY_CONFIG", "__Internal")]
		NSString ODWCFG_MAP_FACTORY_CONFIG { get; }

		// extern NSString *const _Nonnull ODWCFG_STR_FACTORY_HOST;
		[Field ("ODWCFG_STR_FACTORY_HOST", "__Internal")]
		NSString ODWCFG_STR_FACTORY_HOST { get; }

		// extern NSString *const _Nonnull ODWCFG_STR_CONTEXT_SCOPE;
		[Field ("ODWCFG_STR_CONTEXT_SCOPE", "__Internal")]
		NSString ODWCFG_STR_CONTEXT_SCOPE { get; }

		// extern NSString *const _Nonnull ODWCFG_MAP_METASTATS_CONFIG;
		[Field ("ODWCFG_MAP_METASTATS_CONFIG", "__Internal")]
		NSString ODWCFG_MAP_METASTATS_CONFIG { get; }

		// extern NSString *const _Nonnull ODWCFG_INT_METASTATS_INTERVAL;
		[Field ("ODWCFG_INT_METASTATS_INTERVAL", "__Internal")]
		NSString ODWCFG_INT_METASTATS_INTERVAL { get; }

		// extern NSString *const _Nonnull ODWCFG_BOOL_METASTATS_SPLIT;
		[Field ("ODWCFG_BOOL_METASTATS_SPLIT", "__Internal")]
		NSString ODWCFG_BOOL_METASTATS_SPLIT { get; }

		// extern NSString *const _Nonnull ODWCFG_MAP_COMPAT;
		[Field ("ODWCFG_MAP_COMPAT", "__Internal")]
		NSString ODWCFG_MAP_COMPAT { get; }

		// extern NSString *const _Nonnull ODWCFG_BOOL_COMPAT_DOTS;
		[Field ("ODWCFG_BOOL_COMPAT_DOTS", "__Internal")]
		NSString ODWCFG_BOOL_COMPAT_DOTS { get; }

		// extern NSString *const _Nonnull ODWCFG_STR_COMPAT_PREFIX;
		[Field ("ODWCFG_STR_COMPAT_PREFIX", "__Internal")]
		NSString ODWCFG_STR_COMPAT_PREFIX { get; }

		// extern NSString *const _Nonnull ODWCFG_BOOL_HOST_MODE;
		[Field ("ODWCFG_BOOL_HOST_MODE", "__Internal")]
		NSString ODWCFG_BOOL_HOST_MODE { get; }

		// extern NSString *const _Nonnull ODWCFG_MAP_HTTP;
		[Field ("ODWCFG_MAP_HTTP", "__Internal")]
		NSString ODWCFG_MAP_HTTP { get; }

		// extern NSString *const _Nonnull ODWCFG_BOOL_HTTP_MS_ROOT_CHECK;
		[Field ("ODWCFG_BOOL_HTTP_MS_ROOT_CHECK", "__Internal")]
		NSString ODWCFG_BOOL_HTTP_MS_ROOT_CHECK { get; }

		// extern NSString *const _Nonnull ODWCFG_BOOL_HTTP_COMPRESSION;
		[Field ("ODWCFG_BOOL_HTTP_COMPRESSION", "__Internal")]
		NSString ODWCFG_BOOL_HTTP_COMPRESSION { get; }

		// extern NSString *const _Nonnull ODWCFG_MAP_TPM;
		[Field ("ODWCFG_MAP_TPM", "__Internal")]
		NSString ODWCFG_MAP_TPM { get; }

		// extern NSString *const _Nonnull ODWCFG_INT_TPM_MAX_RETRY;
		[Field ("ODWCFG_INT_TPM_MAX_RETRY", "__Internal")]
		NSString ODWCFG_INT_TPM_MAX_RETRY { get; }

		// extern NSString *const _Nonnull ODWCFG_STR_TPM_BACKOFF;
		[Field ("ODWCFG_STR_TPM_BACKOFF", "__Internal")]
		NSString ODWCFG_STR_TPM_BACKOFF { get; }

		// extern NSString *const _Nonnull ODWCFG_INT_TPM_MAX_BLOB_BYTES;
		[Field ("ODWCFG_INT_TPM_MAX_BLOB_BYTES", "__Internal")]
		NSString ODWCFG_INT_TPM_MAX_BLOB_BYTES { get; }

		// extern NSString *const _Nonnull ODWCFG_BOOL_TPM_CLOCK_SKEW_ENABLED;
		[Field ("ODWCFG_BOOL_TPM_CLOCK_SKEW_ENABLED", "__Internal")]
		NSString ODWCFG_BOOL_TPM_CLOCK_SKEW_ENABLED { get; }

		// extern NSString *const _Nonnull ODWCFG_BOOL_SESSION_RESET_ENABLED;
		[Field ("ODWCFG_BOOL_SESSION_RESET_ENABLED", "__Internal")]
		NSString ODWCFG_BOOL_SESSION_RESET_ENABLED { get; }
	}

	// @interface ODWLogConfiguration : NSObject
	[BaseType (typeof(NSObject), Name = "ODWLogConfiguration")]
	[Protocol]
	public interface LogConfiguration
	{
		// +(ODWLogConfiguration * _Nullable)getLogConfigurationCopy;
		[Static]
		[NullAllowed, Export ("getLogConfigurationCopy")]
		LogConfiguration LogConfigurationCopy { get; }

		// +(NSString * _Nullable)eventCollectorUri;
		// +(void)setEventCollectorUri:(NSString * _Nonnull)eventCollectorUri;
		[Static]
		[NullAllowed, Export ("eventCollectorUri")]
		string EventCollectorUri { get; set; }

		// +(uint64_t)cacheMemorySizeLimitInBytes;
		// +(void)setCacheMemorySizeLimitInBytes:(uint64_t)cacheMemorySizeLimitInBytes;
		[Static]
		[Export ("cacheMemorySizeLimitInBytes")]
		ulong CacheMemorySizeLimitInBytes { get; set; }

		// +(uint64_t)cacheFileSizeLimitInBytes;
		// +(void)setCacheFileSizeLimitInBytes:(uint64_t)cacheFileSizeLimitInBytes;
		[Static]
		[Export ("cacheFileSizeLimitInBytes")]
		ulong CacheFileSizeLimitInBytes { get; set; }

		// +(void)setMaxTeardownUploadTimeInSec:(int)maxTeardownUploadTimeInSec;
		[Static]
		[Export ("setMaxTeardownUploadTimeInSec:")]
		void SetMaxTeardownUploadTimeInSec (int maxTeardownUploadTimeInSec);

		// +(void)setTraceLevel:(int)TraceLevel;
		[Static]
		[Export ("setTraceLevel:")]
		void SetTraceLevel (int TraceLevel);

		// +(_Bool)enableTrace;
		// +(void)setEnableTrace:(_Bool)enableTrace;
		[Static]
		[Export ("enableTrace")]
		bool EnableTrace { get; set; }

		// +(_Bool)enableConsoleLogging;
		// +(void)setEnableConsoleLogging:(_Bool)enableConsoleLogging;
		[Static]
		[Export ("enableConsoleLogging")]
		bool EnableConsoleLogging { get; set; }

		// +(_Bool)surfaceCppExceptions;
		// +(void)setSurfaceCppExceptions:(_Bool)surfaceCppExceptions;
		[Static]
		[Export ("surfaceCppExceptions")]
		bool SurfaceCppExceptions { get; set; }

		// +(_Bool)enableSessionReset;
		// +(void)setEnableSessionReset:(_Bool)enableSessionReset;
		[Static]
		[Export ("enableSessionReset")]
		bool EnableSessionReset { get; set; }

		// +(NSString * _Nullable)cacheFilePath;
		// +(void)setCacheFilePath:(NSString * _Nonnull)cacheFilePath;
		[Static]
		[NullAllowed, Export ("cacheFilePath")]
		string CacheFilePath { get; set; }

		// +(_Bool)enableDbCheckpointOnFlush;
		// +(void)setEnableDbCheckpointOnFlush:(_Bool)enableDbCheckpointOnFlush;
		[Static]
		[Export ("enableDbCheckpointOnFlush")]
		bool EnableDbCheckpointOnFlush { get; set; }

		// -(void)set:(NSString * _Nonnull)key withValue:(NSString * _Nonnull)value;
		[Export ("set:withValue:")]
		void Set (string key, string value);

		// -(NSString * _Nullable)valueForKey:(NSString * _Nonnull)key;
		[Export ("valueForKey:")]
		[return: NullAllowed]
		string ValueForKey (string key);

		// -(NSString * _Nullable)host;
		// -(void)setHost:(NSString * _Nonnull)host;
		[NullAllowed, Export ("host")]
		string Host { get; set; }
	}

	// @interface ODWPrivacyGuardInitConfig : NSObject
	[BaseType (typeof(NSObject), Name = "ODWPrivacyGuardInitConfig")]
	[Protocol]
	public interface PrivacyGuardInitConfig
	{
		// @property (readwrite, nonatomic) ODWCommonDataContext * _Nonnull dataContext;
		[Export ("dataContext", ArgumentSemantic.Assign)]
		CommonDataContext DataContext { get; set; }

		// @property (readwrite, copy, nonatomic) NSString * _Nonnull notificationEventName;
		[Export ("notificationEventName")]
		string NotificationEventName { get; set; }

		// @property (readwrite, copy, nonatomic) NSString * _Nonnull semanticContextNotificationEventName;
		[Export ("semanticContextNotificationEventName")]
		string SemanticContextNotificationEventName { get; set; }

		// @property (readwrite, copy, nonatomic) NSString * _Nonnull summaryEventName;
		[Export ("summaryEventName")]
		string SummaryEventName { get; set; }

		// @property (readwrite, nonatomic) BOOL useEventFieldPrefix;
		[Export ("useEventFieldPrefix")]
		bool UseEventFieldPrefix { get; set; }

		// @property (readwrite, nonatomic) BOOL scanForUrls;
		[Export ("scanForUrls")]
		bool ScanForUrls { get; set; }

		// @property (readwrite, nonatomic) BOOL disableAdvancedScans;
		[Export ("disableAdvancedScans")]
		bool DisableAdvancedScans { get; set; }

		// @property (readwrite, nonatomic) BOOL stampEventIKeyForConcerns;
		[Export ("stampEventIKeyForConcerns")]
		bool StampEventIKeyForConcerns { get; set; }
	}

	// @interface ODWSemanticContext : NSObject
	[BaseType (typeof(NSObject), Name = "ODWSemanticContext")]
	[Protocol]
	public interface SemanticContext
	{
		// -(void)setAppId:(NSString * _Nonnull)appId;
		[Export ("setAppId:")]
		void SetAppId (string appId);

		// -(void)setAppVersion:(NSString * _Nonnull)appVersion;
		[Export ("setAppVersion:")]
		void SetAppVersion (string appVersion);

		// -(void)setAppLanguage:(NSString * _Nonnull)appLanguage;
		[Export ("setAppLanguage:")]
		void SetAppLanguage (string appLanguage);

		// -(void)setUserId:(NSString * _Nonnull)userId;
		[Export ("setUserId:")]
		void SetUserId (string userId);

		// -(void)setUserId:(NSString * _Nonnull)userId piiKind:(enum ODWPiiKind)pii;
		[Export ("setUserId:piiKind:")]
		void SetUserId (string userId, PiiKind pii);

		// -(void)setDeviceId:(NSString * _Nonnull)deviceId;
		[Export ("setDeviceId:")]
		void SetDeviceId (string deviceId);

		// -(void)setUserTimeZone:(NSString * _Nonnull)userTimeZone;
		[Export ("setUserTimeZone:")]
		void SetUserTimeZone (string userTimeZone);

		// -(void)setUserAdvertisingId:(NSString * _Nonnull)userAdvertisingId;
		[Export ("setUserAdvertisingId:")]
		void SetUserAdvertisingId (string userAdvertisingId);

		// -(void)setAppExperimentIds:(NSString * _Nonnull)experimentIds;
		[Export ("setAppExperimentIds:")]
		void SetAppExperimentIds (string experimentIds);

		// -(void)setAppExperimentIds:(NSString * _Nonnull)experimentIds forEvent:(NSString * _Nonnull)eventName;
		[Export ("setAppExperimentIds:forEvent:")]
		void SetAppExperimentIds (string experimentIds, string eventName);

		// -(void)setAppExperimentETag:(NSString * _Nonnull)eTag;
		[Export ("setAppExperimentETag:")]
		void SetAppExperimentETag (string eTag);

		// -(void)setAppExperimentImpressionId:(NSString * _Nonnull)impressionId;
		[Export ("setAppExperimentImpressionId:")]
		void SetAppExperimentImpressionId (string impressionId);
	}

	// @interface ODWLogger : NSObject
	[BaseType (typeof(NSObject), Name = "ODWLogger")]
	[Protocol]
	public interface Logger
	{
		// -(void)logEventWithName:(NSString * _Nonnull)name;
		[Export ("logEventWithName:")]
		void LogEvent (string name);

		// -(void)logEventWithEventProperties:(ODWEventProperties * _Nonnull)properties;
		[Export ("logEventWithEventProperties:")]
		void LogEvent (EventProperties properties);

		// -(void)logFailureWithSignature:(NSString * _Nonnull)signature detail:(NSString * _Nonnull)detail eventproperties:(ODWEventProperties * _Nonnull)properties;
		[Export ("logFailureWithSignature:detail:eventproperties:")]
		void LogFailure (string signature, string detail, EventProperties properties);

		// -(void)logFailureWithSignature:(NSString * _Nonnull)signature detail:(NSString * _Nonnull)detail category:(NSString * _Nonnull)category id:(NSString * _Nonnull)identifier eventProperties:(ODWEventProperties * _Nonnull)properties;
		[Export ("logFailureWithSignature:detail:category:id:eventProperties:")]
		void LogFailure (string signature, string detail, string category, string identifier, EventProperties properties);

		// -(void)logPageViewWithId:(NSString * _Nonnull)identifier pageName:(NSString * _Nonnull)pageName eventProperties:(ODWEventProperties * _Nonnull)properties;
		[Export ("logPageViewWithId:pageName:eventProperties:")]
		void LogPageView (string identifier, string pageName, EventProperties properties);

		// -(void)logPageViewWithId:(NSString * _Nonnull)identifier pageName:(NSString * _Nonnull)pageName category:(NSString * _Nonnull)category uri:(NSString * _Nonnull)uri referrerUri:(NSString * _Nonnull)referrerUri eventProperties:(ODWEventProperties * _Nonnull)properties;
		[Export ("logPageViewWithId:pageName:category:uri:referrerUri:eventProperties:")]
		void LogPageView (string identifier, string pageName, string category, string uri, string referrerUri, EventProperties properties);

		// -(void)logTraceWithTraceLevel:(enum ODWTraceLevel)traceLevel message:(NSString * _Nonnull)message eventProperties:(ODWEventProperties * _Nonnull)properties;
		[Export ("logTraceWithTraceLevel:message:eventProperties:")]
		void LogTrace (TraceLevel traceLevel, string message, EventProperties properties);

		// -(void)logSessionWithState:(enum ODWSessionState)state eventProperties:(ODWEventProperties * _Nonnull)properties;
		[Export ("logSessionWithState:eventProperties:")]
		void LogSession (SessionState state, EventProperties properties);

		// -(void)initializePrivacyGuardWithODWPrivacyGuardInitConfig:(ODWPrivacyGuardInitConfig * _Nonnull)initConfigObject;
		[Export ("initializePrivacyGuardWithODWPrivacyGuardInitConfig:")]
		void InitializePrivacyGuard (PrivacyGuardInitConfig initConfigObject);

		// -(void)setContextWithName:(NSString * _Nonnull)name stringValue:(NSString * _Nonnull)value;
		[Export ("setContextWithName:stringValue:")]
		void SetContext (string name, string value);

		// -(void)setContextWithName:(NSString * _Nonnull)name stringValue:(NSString * _Nonnull)value piiKind:(enum ODWPiiKind)piiKind;
		[Export ("setContextWithName:stringValue:piiKind:")]
		void SetContext (string name, string value, PiiKind piiKind);

		// -(void)setContextWithName:(NSString * _Nonnull)name boolValue:(BOOL)value;
		[Export ("setContextWithName:boolValue:")]
		void SetContext (string name, bool value);

		// -(void)setContextWithName:(NSString * _Nonnull)name boolValue:(BOOL)value piiKind:(enum ODWPiiKind)piiKind;
		[Export ("setContextWithName:boolValue:piiKind:")]
		void SetContext (string name, bool value, PiiKind piiKind);

		// -(void)setContextWithName:(NSString * _Nonnull)name dateValue:(NSDate * _Nonnull)value;
		[Export ("setContextWithName:dateValue:")]
		void SetContext (string name, NSDate value);

		// -(void)setContextWithName:(NSString * _Nonnull)name dateValue:(NSDate * _Nonnull)value piiKind:(enum ODWPiiKind)piiKind;
		[Export ("setContextWithName:dateValue:piiKind:")]
		void SetContext (string name, NSDate value, PiiKind piiKind);

		// -(void)setContextWithName:(NSString * _Nonnull)name doubleValue:(double)value;
		[Export ("setContextWithName:doubleValue:")]
		void SetContext (string name, double value);

		// -(void)setContextWithName:(NSString * _Nonnull)name doubleValue:(double)value piiKind:(enum ODWPiiKind)piiKind;
		[Export ("setContextWithName:doubleValue:piiKind:")]
		void SetContext (string name, double value, PiiKind piiKind);

		// -(void)setContextWithName:(NSString * _Nonnull)name int64Value:(int64_t)value;
		[Export ("setContextWithName:int64Value:")]
		void SetContext (string name, long value);

		// -(void)setContextWithName:(NSString * _Nonnull)name int64Value:(int64_t)value piiKind:(enum ODWPiiKind)piiKind;
		[Export ("setContextWithName:int64Value:piiKind:")]
		void SetContext (string name, long value, PiiKind piiKind);

		// -(void)setContextWithName:(NSString * _Nonnull)name int32Value:(int32_t)value;
		[Export ("setContextWithName:int32Value:")]
		void SetContext (string name, int value);

		// -(void)setContextWithName:(NSString * _Nonnull)name int32Value:(int32_t)value piiKind:(enum ODWPiiKind)piiKind;
		[Export ("setContextWithName:int32Value:piiKind:")]
		void SetContext (string name, int value, PiiKind piiKind);

		// -(void)setContextWithName:(NSString * _Nonnull)name UUIDValue:(NSUUID * _Nonnull)value;
		[Export ("setContextWithName:UUIDValue:")]
		void SetContext (string name, NSUuid value);

		// -(void)setContextWithName:(NSString * _Nonnull)name UUIDValue:(NSUUID * _Nonnull)value piiKind:(enum ODWPiiKind)piiKind;
		[Export ("setContextWithName:UUIDValue:piiKind:")]
		void SetContext (string name, NSUuid value, PiiKind piiKind);

		// @property (readonly, nonatomic, strong) ODWSemanticContext * _Nonnull semanticContext;
		[Export ("semanticContext", ArgumentSemantic.Strong)]
		SemanticContext SemanticContext { get; }
	}

	// @interface ODWLogManager : NSObject
	[BaseType (typeof(NSObject), Name = "ODWLogManager")]
	[Protocol]
	public interface LogManager
	{
		// +(ODWLogger * _Nullable)initForTenant:(NSString * _Nonnull)tenantToken;
		[Static]
		[Export ("initForTenant:")]
		[return: NullAllowed]
		Logger InitForTenant (string tenantToken);

		// +(ODWLogger * _Nullable)initForTenant:(NSString * _Nonnull)tenantToken withConfig:(NSDictionary * _Nullable)config;
		[Static]
		[Export ("initForTenant:withConfig:")]
		[return: NullAllowed]
		Logger InitForTenant (string tenantToken, [NullAllowed] NSDictionary config);

		// +(ODWLogger * _Nullable)loggerWithTenant:(NSString * _Nonnull)tenantToken;
		[Static]
		[Export ("loggerWithTenant:")]
		[return: NullAllowed]
		Logger LoggerWithTenant (string tenantToken);

		// +(ODWLogger * _Nullable)loggerWithTenant:(NSString * _Nonnull)tenantToken source:(NSString * _Nonnull)source;
		[Static]
		[Export ("loggerWithTenant:source:")]
		[return: NullAllowed]
		Logger LoggerWithTenant (string tenantToken, string source);

		// +(ODWLogger * _Nullable)loggerWithTenant:(NSString * _Nonnull)tenantToken source:(NSString * _Nonnull)source withConfig:(ODWLogConfiguration * _Nonnull)config;
		[Static]
		[Export ("loggerWithTenant:source:withConfig:")]
		[return: NullAllowed]
		Logger LoggerWithTenant (string tenantToken, string source, LogConfiguration config);

		// +(ODWLogger * _Nullable)loggerForSource:(NSString * _Nonnull)source;
		[Static]
		[Export ("loggerForSource:")]
		[return: NullAllowed]
		Logger LoggerForSource (string source);

		// +(void)uploadNow;
		[Static]
		[Export ("uploadNow")]
		void UploadNow ();

		// +(ODWStatus)flush;
		[Static]
		[Export ("flush")]
		Status Flush ();

		// +(void)setTransmissionProfile:(ODWTransmissionProfile)profile;
		[Static]
		[Export ("setTransmissionProfile:")]
		void SetTransmissionProfile (TransmissionProfile profile);

		// +(void)pauseTransmission;
		[Static]
		[Export ("pauseTransmission")]
		void PauseTransmission ();

		// +(void)resumeTransmission;
		[Static]
		[Export ("resumeTransmission")]
		void ResumeTransmission ();

		// +(ODWStatus)flushAndTeardown;
		[Static]
		[Export("flushAndTeardown")]
		Status FlushAndTeardown ();

		// +(void)resetTransmitProfiles;
		[Static]
		[Export ("resetTransmitProfiles")]
		void ResetTransmitProfiles ();

		// +(void)setContextWithName:(NSString * _Nonnull)name stringValue:(NSString * _Nonnull)value;
		[Static]
		[Export ("setContextWithName:stringValue:")]
		void SetContextWithName (string name, string value);

		// +(void)setContextWithName:(NSString * _Nonnull)name stringValue:(NSString * _Nonnull)value piiKind:(enum ODWPiiKind)piiKind;
		[Static]
		[Export ("setContextWithName:stringValue:piiKind:")]
		void SetContextWithName (string name, string value, PiiKind piiKind);

		// +(void)applicationWillTerminate;
		[Static]
		[Export ("applicationWillTerminate")]
		void ApplicationWillTerminate ();
	}

	// @interface ODWPrivacyGuard : NSObject
	[BaseType (typeof(NSObject), Name = "ODWPrivacyGuard")]
	[Protocol]
	public interface PrivacyGuard
	{
		// +(_Bool)enabled;
		// +(void)setEnabled:(_Bool)enabled;
		[Static]
		[Export ("enabled")]
		bool Enabled { get; set; }

		// +(void)appendCommonDataContext:(ODWCommonDataContext * _Nonnull)freshCommonDataContext;
		[Static]
		[Export ("appendCommonDataContext:")]
		void AppendCommonDataContext (CommonDataContext freshCommonDataContext);

		// +(void)addIgnoredConcern:(NSString * _Nonnull)EventName withNSString:(NSString * _Nonnull)FieldName withODWDataConcernType:(ODWDataConcernType)IgnoredConcern;
		[Static]
		[Export ("addIgnoredConcern:withNSString:withODWDataConcernType:")]
		void AddIgnoredConcern (string EventName, string FieldName, DataConcernType IgnoredConcern);

		// +(void)resetPrivacyGuardInstance;
		[Static]
		[Export ("resetPrivacyGuardInstance")]
		void ResetPrivacyGuardInstance ();
	}
}
