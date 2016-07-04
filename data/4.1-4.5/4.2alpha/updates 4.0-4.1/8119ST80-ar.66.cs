"Change Set:		8119ST80-ar.66ST80-ar.66:Remove support for isolation layers.ST80-nice.63:Fix from http://bugs.squeak.org/view.php?id=7071 for unicode character inputST80-dtl.64:Continue factoring Project into MVCProject and MorphicProject. Add method category 'enter' for methods associated with entering one project from another, including MVC-Morphic transition. Project>>enter: revert:saveForRevert: is significantly modified. Changes are in packages System, Morphic, and ST-80.ST80-dtl.65:Factor Project>>saveState into MVCProject and MorphicProject."!ScrollController subclass: #ParagraphEditor	instanceVariableNames: 'paragraph startBlock stopBlock beginTypeInBlock emphasisHere initialText selectionShowing otherInterval lastParentLocation wasComposition'	classVariableNames: 'ChangeText CmdActions FindText Keyboard ShiftCmdActions UndoInterval UndoMessage UndoParagraph UndoSelection Undone'	poolDictionaries: 'TextConstants'	category: 'ST80-Controllers'!!ParagraphEditor methodsFor: 'accessing-selection' stamp: 'yo 10/10/2007 20:34'!zapSelectionWith: aText	"Deselect, and replace the selection text by aText.	 Remember the resulting selectionInterval in UndoInterval and otherInterval.	 Do not set up for undo."	| start stop |	self deselect.	start := self startIndex.	stop := self stopIndex.	(aText isEmpty and: [stop > start]) ifTrue:		["If deleting, then set emphasisHere from 1st character of the deletion"		emphasisHere := (paragraph text attributesAt: start forStyle: paragraph textStyle)					select: [:att | att mayBeExtended]].	(start = stop and: [aText size = 0]) ifFalse:		[paragraph			replaceFrom: start			to: stop - 1			with: aText			displaying: true.		self computeIntervalFrom: start to: start + aText size - 1.		self wasComposition ifTrue: [wasComposition := false. self setPoint: start + 1].		UndoInterval := otherInterval := self selectionInterval]! !!ParagraphEditor methodsFor: 'accessing-selection' stamp: 'yo 10/10/2007 20:13'!charBefore	| start |	(start := self startIndex) > 1 ifTrue: [^ paragraph text at: start - 1].	^ nil.! !!ParagraphEditor methodsFor: 'accessing-selection' stamp: 'yo 10/16/2007 21:01'!zapSelectionWithCompositionWith: aString	"Deselect, and replace the selection text by aString.	 Remember the resulting selectionInterval in UndoInterval and otherInterval.	 Do not set up for undo."	| stream newString aText beforeChar |	wasComposition := false.	((aString isEmpty or: [(beforeChar := self charBefore) isNil]) or: [		aString size = 1 and: [(Unicode isComposition: aString first) not]]) ifTrue: [			^ self zapSelectionWith: (Text string: aString emphasis: emphasisHere)].	stream := UnicodeCompositionStream on: (String new: 16).	stream nextPut: beforeChar.	stream nextPutAll: aString.	newString := stream contents.	aText := Text string: newString emphasis: emphasisHere.	self markBlock < self pointBlock		ifTrue: [self setMark: self markBlock stringIndex - 1]		ifFalse: [self setPoint: self  pointBlock stringIndex - 1].	wasComposition := true. 	self zapSelectionWith: aText.! !!ScreenController methodsFor: 'nested menus' stamp: 'ar 11/12/2009 01:07'!changesMenu	"Answer a menu for changes-related items"	^ SelectionMenu labelList:		#(			'simple change sorter'			'dual change sorter'			'file out current change set'			'create new change set...'			'browse changed methods'			'check change set for slips'			'browse recent submissions'			'recently logged changes...'			'recent log file...'			)		lines: #(1 3 7)		selections: #(openSimpleChangeSorter openChangeManagerfileOutChanges newChangeSet browseChangedMessages lookForSlipsbrowseRecentSubmissions browseRecentLog fileForRecentLog)"ScreenController new changesMenu startUp"! !!ParagraphEditor methodsFor: 'typing support' stamp: 'yo 10/10/2007 20:32'!readKeyboard	"Key struck on the keyboard. Find out which one and, if special, carry 	out the associated special action. Otherwise, add the character to the 	stream of characters.  Undoer & Redoer: see closeTypeIn."	| typeAhead char |	typeAhead := WriteStream on: (String new: 128).	[sensor keyboardPressed] whileTrue: 		[self deselect.		 [sensor keyboardPressed] whileTrue: 			[char := sensor keyboardPeek.			(self dispatchOnCharacter: char with: typeAhead) ifTrue:				[self doneTyping.				self setEmphasisHere.				^self selectAndScroll; updateMarker].			self openTypeIn].		self hasSelection ifTrue: "save highlighted characters"			[UndoSelection := self selection]. 		self zapSelectionWithCompositionWith: typeAhead contents.		typeAhead reset.		self unselect.		sensor keyboardPressed ifFalse: 			[self selectAndScroll.			sensor keyboardPressed				ifFalse: [self updateMarker]]]! !!ParagraphEditor methodsFor: 'parenblinking' stamp: 'nice 8/21/2008 18:27'!dispatchOnCharacter: char with: typeAheadStream	"Carry out the action associated with this character, if any.	Type-ahead is passed so some routines can flush or use it."	| honorCommandKeys |	self clearParens.  	"mikki 1/3/2005 21:31 Preference for auto-indent on return added."	char asciiValue = 13 ifTrue: [		^Preferences autoIndent 			ifTrue: [				sensor controlKeyPressed					ifTrue: [self normalCharacter: typeAheadStream]					ifFalse: [self crWithIndent: typeAheadStream]]			ifFalse: [				sensor controlKeyPressed					ifTrue: [self crWithIndent: typeAheadStream]					ifFalse: [self normalCharacter: typeAheadStream]]].	((honorCommandKeys := Preferences cmdKeysInText) and: [char = Character enter])		ifTrue: [^ self dispatchOnEnterWith: typeAheadStream].			"Special keys overwrite crtl+key combinations - at least on Windows. To resolve this	conflict, assume that keys other than cursor keys aren't used together with Crtl." 	((self class specialShiftCmdKeys includes: char asciiValue) and: [char asciiValue < 27])		ifTrue: [^ sensor controlKeyPressed			ifTrue: [self perform: (ShiftCmdActions at: char asciiValue + 1) with: typeAheadStream]			ifFalse: [self perform: (CmdActions at: char asciiValue + 1) with: typeAheadStream]].	"backspace, and escape keys (ascii 8 and 27) are command keys"	((honorCommandKeys and: [sensor commandKeyPressed]) or: [self class specialShiftCmdKeys includes: char asciiValue]) ifTrue:		[^ sensor leftShiftDown			ifTrue:				[self perform: (ShiftCmdActions at: char asciiValue + 1 ifAbsent: [#noop:]) with: typeAheadStream]			ifFalse:				[self perform: (CmdActions at: char asciiValue + 1 ifAbsent: [#noop:]) with: typeAheadStream]].	"the control key can be used to invoke shift-cmd shortcuts"	(honorCommandKeys and: [sensor controlKeyPressed])		ifTrue:			[^ self perform: (ShiftCmdActions at: char asciiValue + 1 ifAbsent: [#noop:]) with: typeAheadStream].	(')]}' includes: char)		ifTrue: [self blinkPrevParen].	^ self perform: #normalCharacter: with: typeAheadStream! !!ParagraphEditor methodsFor: 'accessing-selection' stamp: 'yo 10/10/2007 20:24'!wasComposition	^ wasComposition ifNil: [^ false].! !!MVCProject methodsFor: 'enter' stamp: 'dtl 11/7/2009 20:18'!setWorldForEmergencyRecovery	"Prepare world for enter with an absolute minimum of mechanism.	An unrecoverable error has been detected in an isolated project."	World := nil.	Smalltalk at: #ScheduledControllers put: world.	ScheduledControllers restore! !!MVCProject methodsFor: 'file in/out' stamp: 'dtl 11/2/2009 23:37'!armsLengthCommand: aCommand withDescription: aString	| pvm |	"Set things up so that this aCommand is sent to self as a messageafter jumping to the parentProject.  For things that can't be executedwhile in this project, such as saveAs, loadFromServer, storeOnServer.  SeeProjectViewMorph step."	parentProject ifNil: [^ self inform: 'The top project can''t do that'].	pvm := parentProject findProjectView: self.	pvm armsLengthCommand: {self. aCommand}.	self exit! !!MVCProject methodsFor: 'enter' stamp: 'dtl 11/7/2009 20:22'!setWorldForEnterFrom: old recorder: recorderOrNil	"Prepare world for enter."	World := nil.  "Signifies MVC"	Smalltalk at: #ScheduledControllers put: world! !!MVCProject methodsFor: 'enter' stamp: 'dtl 11/9/2009 21:34'!saveState	"Save the current state in me prior to leaving this project"	changeSet := ChangeSet current.	thumbnail ifNotNil: [thumbnail hibernate].	world := ScheduledControllers.	ScheduledControllers unCacheWindows.	Sensor flushAllButDandDEvents. "Will be reinstalled by World>>install"	transcript := Transcript! !!MVCProject methodsFor: 'display' stamp: 'dtl 11/6/2009 21:57'!viewLocFor: exitedProject 	"Look for a view of the exitedProject, and return its center"	(world controllerWhoseModelSatisfies: [:p | p == exitedProject])		ifNotNilDo: [:ctlr | ^ctlr view windowBox center].	^Sensor cursorPoint	"default result"! !!MVCProject methodsFor: 'enter' stamp: 'dtl 11/7/2009 20:55'!pauseSoundPlayers	"Pause sound players, subject to preference settings"	Smalltalk at: #ScorePlayer		ifPresentAndInMemory: [:playerClass | playerClass				allSubInstancesDo: [:player | player pause]]! !!MVCProject methodsFor: 'enter' stamp: 'dtl 11/7/2009 20:34'!scheduleProcessForEnter: showZoom	"Complete the enter: by launching a new process"	| newProcess |	SystemWindow clearTopWindow.	"break external ref to this project"	newProcess := [			ScheduledControllers resetActiveController.	"in case of walkback in #restore"		showZoom ifFalse: [ScheduledControllers restore].		ScheduledControllers searchForActiveController	] fixTemps newProcess priority: Processor userSchedulingPriority.	newProcess resume.		"lose the current process and its referenced morphs"	Processor terminateActive! !ScreenController removeSelector: #propagateChanges!ScreenController removeSelector: #beIsolated!