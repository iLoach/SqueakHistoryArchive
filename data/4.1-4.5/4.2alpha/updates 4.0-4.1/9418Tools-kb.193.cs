"Change Set:		9418Tools-kb.193Tools-kb.193: - changed the wording of the workspace's toggleStylingLabel to lowercase.Tools-ul.190:- integrate MessageTally fixes from Cuis (by Juan Vuletich)Tools-kb.191: - Added code styling to Workspace. Code adapted from SHWorkspaceTools-kb.192: - Added shout styling to Workspace. Code adapted from SHWorkspace.  - Added a preference to enable/disable styling in workspaces. - Added a menu item to the yellowButtonMenu of workspaces to toggle styling."!StringHolder subclass: #Workspace	instanceVariableNames: 'bindings acceptDroppedMorphs acceptAction mustDeclareVariables shouldStyle'	classVariableNames: 'ShouldStyle'	poolDictionaries: ''	category: 'Tools-Base'!!FileList2 class methodsFor: 'morphic ui' stamp: 'ul 2/22/2010 22:22'!morphicViewFileSelectorForSuffixes: aList 	"Answer a morphic file-selector tool for the given suffix list."		^self morphicViewFileSelectorForSuffixes: aList directory: self lastSelDir! !!Workspace methodsFor: 'binding' stamp: 'kb 2/22/2010 22:29'!hasBindingOf: aString 		^bindings notNil and: [ bindings includesKey: aString ]! !!Workspace methodsFor: 'code pane menu' stamp: 'kb 2/23/2010 00:55'!toggleStylingLabel	^self shouldStyle 		ifTrue: [ 'disable shout styling' ]		ifFalse: [ 'enable shout styling' ]! !!Workspace methodsFor: 'binding' stamp: 'kb 2/22/2010 22:18'!hasBindingThatBeginsWith: aString 		bindings ifNil: [ ^false ].	bindings keysDo: [ :each |		(each beginsWith: aString) ifTrue: [ ^true ] ].	^false! !!Workspace methodsFor: 'toolbuilder' stamp: 'kb 2/22/2010 22:12'!buildCodePaneWith: builder	| textSpec |	textSpec := builder pluggableCodePaneSpec new.	textSpec 		model: self;		getText: #contents; 		setText: #contents:notifying:; 		selection: #contentsSelection; 		menu: #codePaneMenu:shifted:.	^textSpec! !!Workspace class methodsFor: 'preferences' stamp: 'kb 2/22/2010 23:56'!shouldStyle: aBoolean	ShouldStyle := aBoolean! !!Workspace methodsFor: 'code pane menu' stamp: 'kb 2/23/2010 00:44'!toggleStyling	shouldStyle := self shouldStyle not.	" Ugly hack, to restyle our contents. "	self codeTextMorph in: [ :codeTextMorph |		codeTextMorph setText:			codeTextMorph textMorph text asString asText ]! !!FileList2 methodsFor: 'private' stamp: 'ul 2/22/2010 22:32'!okHit	ok := true.	currentDirectorySelected		ifNil: [ Beeper beep ]		ifNotNil: [			self class lastSelDir: directory.			modalView delete ]! !!FileList2 class methodsFor: 'accessing' stamp: 'ul 2/22/2010 22:32'!lastSelDir	"Return the last selected directory or the default directory if no directory was selected so far."	^lastSelDir ifNil: [ lastSelDir := FileDirectory default ]! !!FileList2 class methodsFor: 'modal dialogs' stamp: 'ul 2/22/2010 22:21'!modalFolderSelector	^self modalFolderSelector: self lastSelDir	! !!Workspace methodsFor: 'code pane menu' stamp: 'kb 2/22/2010 23:53'!addToggleStylingMenuItemTo: aMenu		aMenu		addUpdating: #toggleStylingLabel		target: self		action: #toggleStyling! !!Workspace methodsFor: 'code pane menu' stamp: 'kb 2/22/2010 23:53'!shouldStyle	^shouldStyle ifNil: [ self class shouldStyle ]! !!Workspace methodsFor: 'code pane menu' stamp: 'kb 2/23/2010 00:50'!codePaneMenu: aMenu shifted: shifted		shifted ifFalse: [ 		self addToggleStylingMenuItemTo: aMenu ].	super codePaneMenu: aMenu shifted: shifted.	^aMenu! !!Workspace class methodsFor: 'preferences' stamp: 'kb 2/22/2010 23:57'!shouldStyle	<preference: 'Shout styling in Workspace' 		category: 'browsing' 		description: 'After enabled, new workspaces use shout to style their contents.' 		type: #Boolean>	^ShouldStyle ifNil: [ ^true ]! !!Debugger methodsFor: 'private' stamp: 'ul 2/22/2010 17:10'!process: aProcess controller: aController context: aContext	super initialize.	Smalltalk at: #MessageTally ifPresentAndInMemory: #terminateTimerProcess.	contents := nil. 	interruptedProcess := aProcess.	interruptedController := aController.	contextStackTop := aContext.	self newStack: (contextStackTop stackOfSize: 1).	contextStackIndex := 1.	externalInterrupt := false.	selectingPC := true.	Smalltalk isMorphic ifTrue:		[errorWasInUIProcess := false]! !!TimeProfileBrowser methodsFor: 'private' stamp: 'jmv 2/19/2010 14:42'!runBlock: aBlock pollingEvery: pollPeriod 	| stream list result |	block := MessageSend 				receiver: self				selector: #runBlock:pollingEvery:				arguments: { 						aBlock.						pollPeriod}.	"so we can re-run it"	tally := MessageTally new.	tally		reportOtherProcesses: false;		maxClassNameSize: 1000;		maxClassPlusSelectorSize: 1000;		maxTabs: 100.	result := tally spyEvery: pollPeriod on: aBlock.	stream := ReadWriteStream 				with: (String streamContents: [ :s | 					tally report: s]).	stream reset.	list := OrderedCollection new.	[stream atEnd] whileFalse: [list add: stream nextLine].	self initializeMessageList: list.	self changed: #messageList.	self changed: #messageListIndex.	^result! !!TimeProfileBrowser methodsFor: 'private' stamp: 'jmv 2/19/2010 14:42'!runProcess: aProcess forMilliseconds: msecDuration pollingEvery: pollPeriod 	| stream list result |	block := MessageSend 				receiver: self				selector: #runProcess:forMilliseconds:pollingEvery: 				arguments: { 						aProcess.						msecDuration.						pollPeriod}.	"so we can re-run it"	tally := MessageTally new.	tally		reportOtherProcesses: false;		maxClassNameSize: 1000;		maxClassPlusSelectorSize: 1000;		maxTabs: 100.	result := tally 				spyEvery: pollPeriod				onProcess: aProcess				forMilliseconds: msecDuration.	stream := ReadWriteStream 				with: (String streamContents: [ :s | 							tally report: s]).	stream reset.	list := OrderedCollection new.	[stream atEnd] whileFalse: [list add: stream nextLine].	self initializeMessageList: list.	self changed: #messageList.	self changed: #messageListIndex.	^result! !!FileList2 class methodsFor: 'modal dialogs' stamp: 'ul 2/22/2010 22:28'!modalFolderSelector: aDir	| window fileModel |	window _ self morphicViewFolderSelector: aDir.	fileModel _ window model.	window openInWorld: self currentWorld extent: 300@400.	self modalLoopOn: window.	^fileModel getSelectedDirectory withoutListWrapper! !!FileList2 class methodsFor: 'accessing' stamp: 'ul 2/22/2010 22:34'!lastSelDir: aFileDirectory	"Store the last selected directory. This will be selected as default in newly opened file or folder selectors"		^lastSelDir := aFileDirectory! !!Workspace methodsFor: 'code pane' stamp: 'kb 2/22/2010 23:58'!aboutToStyle: aStyler	self shouldStyle ifFalse: [ ^false ].	aStyler 		classOrMetaClass: nil;		workspace: self.	^true! !