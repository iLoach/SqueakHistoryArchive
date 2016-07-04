"Change Set:		8792System-ar.220System-ar.220:Make Protocols unloadable: Avoid dependencies on Vocabulary."!!Utilities class methodsFor: 'closure support' stamp: 'ar 1/4/2010 02:32'!postRecompileCleanup	"Utilities postRecompileCleanup"	"Cleanup after loading closure bootstrap"	| unboundMethods contexts |	ProcessorScheduler startUp.	WeakArray restartFinalizationProcess.	MethodChangeRecord allInstancesDo:[:x| x noteNewMethod: nil].	Undeclared removeUnreferencedKeys.	Delay startTimerEventLoop.	EventSensor install.	WorldState allInstancesDo:[:ws| ws convertAlarms; convertStepList].	(Workspace canUnderstand: #initializeBindings) 		ifTrue:[Workspace allInstancesDo:[:ws| ws initializeBindings]].	ExternalDropHandler initialize.	ScrollBar initializeImagesCache.	Smalltalk at: #Vocabulary ifPresent:[:aClass| aClass initialize].	Smalltalk garbageCollect.	GradientFillStyle initPixelRampCache.	Smalltalk at: #ServiceGui ifPresent:[:sg| sg initialize].	Smalltalk		at: #SokobanMorph		ifPresent: [:sm| sm initFields].	Smalltalk		at: #DebuggerMethodMap		ifPresent: [:dmm| dmm voidMapCache].	Smalltalk		at: #KClipboard		ifPresent: [:kcb| kcb clearDefault].	Smalltalk		at: #ServiceRegistry		ifPresent: [:sr| sr rebuild].	(ProcessBrowser respondsTo: #registerWellKnownProcesses) ifTrue:		[ProcessBrowser registerWellKnownProcesses].	Smalltalk		at: #DebuggerMethodMap		ifPresent: [:dmm| dmm voidMapCache].	Smalltalk at: #ServiceRegistry ifPresent:[:cls| cls rebuild].	Smalltalk forgetDoIts.	Smalltalk garbageCollect.	unboundMethods := CompiledMethod allInstances select:[:m|		m methodClass isNil or: [m ~~ (m methodClass compiledMethodAt: m selector ifAbsent: nil)]].	unboundMethods := unboundMethods reject:[:m| m selector isDoIt].	unboundMethods notEmpty ifTrue:		[(ToolSet inspect: unboundMethods) setLabel: 'Unbound Methods'].	contexts := BlockContext allInstances.	contexts ifNotEmpty:[contexts inspect. self inform: 'There are left-over BlockContexts'].	(unboundMethods isEmpty and:[contexts isEmpty]) ifTrue:[		self inform:'Congratulations - The bootstrap is now complete.'.	].! !Locale class removeSelector: #migrateSystem!