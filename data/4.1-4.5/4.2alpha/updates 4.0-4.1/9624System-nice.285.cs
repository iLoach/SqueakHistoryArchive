"Change Set:		9624System-nice.285System-nice.285:merge ar.284System-ar.282:Final touches. Done. Phew.System-dtl.283:New class comment for SmalltalkImage:I represent the current image and runtime environment, including system organization, the virtual machine, object memory, plugins and source files. My instance variable #globals is a reference to the system dictionary of global variables and class names.My singleton instance is called Smalltalk.System-ar.284:Merge System-dtl.283, System-ar.283System-nice.284:1) Change SystemDictionary comment.2) add new accessor to system attribute: #buildDate"!Object subclass: #SmalltalkImage	instanceVariableNames: 'globals'	classVariableNames: 'EndianCache LastImageName LastQuitLogPosition LastStats LowSpaceProcess LowSpaceSemaphore MemoryHogs ShutDownList SourceFileVersionString SpecialSelectors StartUpList StartupStamp WordSize'	poolDictionaries: ''	category: 'System-Support'!IdentityDictionary subclass: #SystemDictionary	instanceVariableNames: 'cachedClassNames'	classVariableNames: ''	poolDictionaries: ''	category: 'System-Support'!!SmalltalkImage methodsFor: 'system attribute' stamp: 'nice 3/6/2010 20:57'!buildDate				"Return a String reflecting the build date of the VM"	"Smalltalk buildDate"	^self getSystemAttribute: 1006! !!SmalltalkImage class methodsFor: 'instance creation' stamp: 'ar 3/5/2010 21:52'!current	"Deprecated. Use Smalltalk instead."	^Smalltalk! !!AbstractLauncher class methodsFor: 'private' stamp: 'ar 3/5/2010 21:50'!extractParameters	| pName value index globals |	globals := Dictionary new.	index := 3.	[pName := Smalltalk  getSystemAttribute: index.	pName isEmptyOrNil] whileFalse:[		index := index + 1.		value := Smalltalk getSystemAttribute: index.		value ifNil: [value := '']. 		globals at: pName asUppercase put: value.		index := index + 1].	^globals! !!SmalltalkImage class methodsFor: 'class initialization' stamp: 'ar 3/5/2010 21:52'!initialize	"SmalltalkImage initialize"	self initializeStartUpList.	self initializeShutDownList.! !!SmalltalkImage class methodsFor: 'instance creation' stamp: 'ar 3/5/2010 21:52'!new	self error: 'Use Smalltalk'.! !!SmalltalkImage methodsFor: 'shrinking' stamp: 'ar 3/6/2010 11:26'!unloadAllKnownPackages	"Unload all packages we know how to unload and reload"	"Prepare unloading"	Smalltalk zapMVCprojects.	Flaps disableGlobalFlaps: false.	StandardScriptingSystem removeUnreferencedPlayers.	Project removeAllButCurrent.	#('Morphic-UserObjects' 'EToy-UserObjects' 'Morphic-Imported' )		do: [:each | SystemOrganization removeSystemCategory: each].	Smalltalk at: #ServiceRegistry ifPresent:[:aClass|		SystemChangeNotifier uniqueInstance			noMoreNotificationsFor: aClass.	].	World removeAllMorphs.	"Go unloading"	#(	'ReleaseBuilder' 'ScriptLoader'		'311Deprecated' '39Deprecated'		'Universes' 'SMLoader' 'SMBase' 'Installer-Core'		'VersionNumberTests' 'VersionNumber'		'Services-Base' 'PreferenceBrowser' 'Nebraska'		'ToolBuilder-MVC' 'ST80'		'CollectionsTests' 'GraphicsTests' 'KernelTests'  'MorphicTests' 		'MultilingualTests' 'NetworkTests' 'ToolsTests' 'TraitsTests'		'SystemChangeNotification-Tests' 'FlexibleVocabularies' 		'EToys' 'Protocols' 'XML-Parser' 'Tests' 'SUnitGUI'	) do:[:pkgName| (MCPackage named: pkgName) unload].	"Traits use custom unload"	Smalltalk at: #Trait ifPresent:[:aClass| aClass unloadTraits].	"Post-unload cleanup"	PackageOrganizer instVarNamed: 'default' put: nil.	SystemOrganization removeSystemCategory: 'UserObjects'.	Presenter defaultPresenterClass: nil.	World dumpPresenter.	ScheduledControllers := nil.	Preferences removePreference: #allowEtoyUserCustomEvents.	SystemOrganization removeEmptyCategories.	ChangeSet removeChangeSetsNamedSuchThat:[:cs | (cs == ChangeSet current) not].	Undeclared removeUnreferencedKeys.	StandardScriptingSystem initialize.	MCFileBasedRepository flushAllCaches.	MCDefinition clearInstances.	Behavior flushObsoleteSubclasses.	ChangeSet current clear.	ChangeSet current name: 'Unnamed1'.	Smalltalk flushClassNameCache.	Smalltalk at: #Browser ifPresent:[:br| br initialize].	DebuggerMethodMap voidMapCache.	DataStream initialize.	Smalltalk forgetDoIts.	AppRegistry removeObsolete.	FileServices removeObsolete.	Preferences removeObsolete.	TheWorldMenu removeObsolete.	Smalltalk garbageCollect.	Symbol compactSymbolTable.	TheWorldMainDockingBar updateInstances.	MorphicProject defaultFill: (Color gray: 0.9).	World color: (Color gray: 0.9).! !SmalltalkImage removeSelector: #extractParameters!SmalltalkImage initialize!