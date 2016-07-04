"Change Set:		8915Morphic-dtl.315Morphic-dtl.315:Merge Morphic-ul.314 and Morphic-dtl.314Morphic-ar.299:Move lastKeystroke back into Morphic.Morphic-ar.300:http://bugs.squeak.org/view.php?id=6581Image freezes (background processes like Seaside make no progress) and Squeak hoggs CPU Morphic-jcg.301:Following Bert's suggestion in the 'Future Sends' thread, use MorphicAlarms instead of forking a Process that immediately waits on a delay before scheduling them with the other deferred messages.This is Morphic-only behavior (implemented in MorphicProject), since other types of Projects don't have the MorphicAlarm mechanism available to them.(ignore Morphic-jcg.280, which is the same changes, but applied agains Morphic-dtl.279).Morphic-jcg.302:After version 301, sending #future from a non-UI thread is no longer thread-safe, because accesses to the MorphicAlarmQueue were not synchronized.  Now they are, for all uses of Morphic alarms (i.e. even non-future-send users can now safely use Morphic alarms from other threads).Morphic-ar.303:Replace offerMenuFrom:shifted: by offerMenu:from:shifted: which takes an additional argument, the model to retrieve the menu from and perform the actions on.Morphic-ul.304:- code criticsMorphic-ar.305:- Promote isScriptEditorMorph to Object.- Fix typo in prepareToBeSaved.Morphic-ar.306:DockingBarMorph should be sticky by default to prevent accidental pickup. For deliberate pickup, use the halo.Morphic-nice.307:let handleEdit: answer the result of evaluating its block argumentMorphic-nice.308:move temp assignment outside blockMorphic-ar.309:Allow changing the text of a PluggableText from the model using 'self changed: #editString with: aString'.Morphic-ar.310:Preps for merging Cobalt's ToolBuilder-Morphic changes:Extend UserDialogBoxMorph with confirm:trueChoice:falseChoice: and fix a small but relevant buglet in flushing a PLMs list when an unchanged getListElementSelector is installed. Morphic-ar.311:Merging Morphic-nice.277:Experimental: let a Rectangle merge in place (I called this swallow:)This has two advantages:- avoid repeated Object creation when we just want the gross result- avoid closures writing to outer tempsIMHO, generalizing this kind of policy could have a measurable impact on GUI speed.However, this is against current policy to never change a Point nor rectangle in place, so I let gurus judge if worth or not.Morphic-dtl.312:Remove all MVC BitEditor references from non-MVC packages.  Form>>bitEdit to Project class>>bitEdit:  Form>>bitEditAt:scale: to Project class>>bitEdit:at:scale:  BitEditor class>>locateMagnifiedView:scale: to Rectangle class>>locateMagnifiedView:scale:Note: StrikeFont>>edit: now notifies user to edit strike fonts from an MVC project.Help needed: There is no Morphic editor for strike fonts, see implementors of #editCharacter:ofFont:Morphic-dtl.313:Remove remaining dependencies on ST80-Editors from non-MVC packages.Remove explicit references to ST80 classes from ModalSystemWindow and various utility methods.Morphic-ul.314:- fix: http://bugs.squeak.org/view.php?id=7449Implement TextEditor >> #selectionAsStream just like ParagraphEditor >> #selectionAsStream. The system expects that the stream contains the full text of the editor, but only the selection can be read from it.- fix: SmalltalkEditor >> #tallyItMorphic-dtl.314:Support Project>>dispatchTo:addPrefixAndSend:withArguments:Add PopUpMenu>>morphicStartUpLeftFlushAdd PopUpMenu>>morphicStartUpWithCaption:icon:at:allowKeyboard:"!MessageSend subclass: #MorphicAlarm	instanceVariableNames: 'scheduledTime sequenceNumber numArgs'	classVariableNames: ''	poolDictionaries: ''	category: 'Morphic-Events'!Heap subclass: #MorphicAlarmQueue	instanceVariableNames: 'mutex sequenceNumber'	classVariableNames: ''	poolDictionaries: ''	category: 'Morphic-Events'!!MorphicProject methodsFor: 'dispatching' stamp: 'dtl 1/30/2010 15:50'!selectorPrefixForDispatch	"A string to be prepended to selectors for project specific methods"	^ 'morphic'! !!DockingBarMorph methodsFor: 'initialization' stamp: 'ar 1/12/2010 18:54'!initialize	"initialize the receiver"	super initialize.	""	selectedItem := nil.	activeSubMenu := nil.	fillsOwner := true.	avoidVisibleBordersAtEdge := true.	autoGradient := Preferences gradientMenu.	""	self setDefaultParameters.	""	self beFloating; beSticky.	""	self layoutInset: 0.	! !!FontChooserTool methodsFor: 'toolbuilder' stamp: 'ul 1/11/2010 07:17'!contents	| sample i c f |	sample := WriteStream on: ''.	f := self selectedFont ifNil:[^Text new].	f isSymbolFont ifFalse:[		sample 			nextPutAll: 'the quick brown fox jumps over the lazy dog' ;cr;			nextPutAll:  'THE QUICK BROWN FOX JUMPS OVER THE LAZY DOG.' ;cr;cr;			nextPutAll: '0123456789'; cr; cr;			nextPutAll: 'Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.'	] ifTrue:[		i := 0.		33 to: 255 do:[:ci |			sample nextPut: (c:=Character value: ci).			i := i + 1.			(('@Z`z' includes:c) or:[i = 30]) 				ifTrue:[i :=0. sample cr]].	].	sample := sample contents asText.	sample addAttribute: (TextFontReference toFont: f).	^sample! !!MorphicProject methodsFor: 'futures' stamp: 'jcg 1/9/2010 13:12'!future: receiver do: aSelector at: deltaMSecs args: args	"Send a message deltaSeconds into the future.  No response is expected."	| msg |	msg := MessageSend receiver: receiver selector: aSelector arguments: args.	deltaMSecs = 0 		ifTrue: [self addDeferredUIMessage: msg]		ifFalse: [			world 				addAlarm: #addDeferredUIMessage: 				withArguments: {msg}				for: self				at: (Time millisecondClockValue + deltaMSecs)		]..	^nil! !!SelectionMorph methodsFor: 'drawing' stamp: 'nice 12/27/2009 22:39'!drawOn: aCanvas	| canvas form1 form2 box |	super drawOn: aCanvas.	box := self bounds copy.	selectedItems do: [:m | box swallow: m fullBounds].	box := box expandBy: 1.	canvas := Display defaultCanvasClass extent: box extent depth: 8.	canvas translateBy: box topLeft negated		during: [:tempCanvas | selectedItems do: [:m | tempCanvas fullDrawMorph: m]].	form1 := (Form extent: box extent) copyBits: (0@0 extent: box extent) from: canvas form at: 0@0 colorMap: (Color maskingMap: 8).	form2 := Form extent: box extent.	(0@0) fourNeighbors do: [:d | form1 displayOn: form2 at: d rule: Form under].	form1 displayOn: form2 at: 0@0 rule: Form erase.	aCanvas stencil: form2		at: box topLeft		sourceRect: form2 boundingBox		color: self borderColor! !!UserDialogBoxMorph class methodsFor: 'utilities' stamp: 'ar 1/23/2010 14:38'!confirm: aString title: titleString trueChoice: trueChoice falseChoice: falseChoice at: aPointOrNil	"UserDialogBoxMorph confirm: 'Make your choice carefully' withCRs title: 'Do you like chocolate?' trueChoice: 'Oh yessir!!' falseChoice: 'Not so much...'"	^self new		title: titleString;		label: aString;		addSelectedButton: '   ', trueChoice translated, '   ' value: true;		addCancelButton: '   ', falseChoice translated, '   '  value: false;		runModalIn: ActiveWorld forHand: ActiveHand at: aPointOrNil! !!MorphicAlarmQueue methodsFor: 'adding' stamp: 'jcg 1/9/2010 13:03'!add: aMorphicAlarm	(sequenceNumber := sequenceNumber + 1) == 16r3FFFFFFF ifTrue: [		"Sequence number overflow... reassign sequence numbers starting at 0."		| alarmList |		alarmList := self asArray sort: [:msg1 :msg2 |			 msg1 sequenceNumber < msg2 sequenceNumber		].		alarmList withIndexDo: [:msg :ind | msg sequenceNumber: ind-1].		"The #bitAnd: for the unlikely event that we have > 16r3FFFFFF messages in the queue."		sequenceNumber := alarmList last sequenceNumber + 1 bitAnd: 16r3FFFFFFF.	].	aMorphicAlarm sequenceNumber: sequenceNumber.	super add: aMorphicAlarm.		"If we doubt our sanity..."	false ifTrue: [		self isValidHeap ifFalse: [self error: 'not a valid heap!!!!!!'].	]! !!PopUpMenu methodsFor: '*Morphic-Menus' stamp: 'dtl 1/30/2010 16:13'!morphicStartUpLeftFlush	"Build and invoke this menu with no initial selection.  By Jerry Archibald, 4/01.	If in MVC, align menus items with the left margin.	Answer the selection associated with the menu item chosen by the user or nil if none is chosen.  	The mechanism for getting left-flush appearance in mvc leaves a tiny possibility for misadventure: if the user, in mvc, puts up the jump-to-project menu, then hits cmd period while it is up, then puts up a second jump-to-project menu before dismissing or proceeding through the debugger, it's possible for mvc popup-menus thereafter to appear left-aligned rather than centered; this very unlikely condition can be cleared by evaluating 'PopUpMenu alignment: 2'"	^self startUp! !!UserDialogBoxMorph class methodsFor: 'utilities' stamp: 'ar 1/23/2010 14:37'!confirm: aString trueChoice: trueChoice falseChoice: falseChoice	"UserDialogBoxMorph confirm: 'Do you like chocolate?' trueChoice: 'Oh yessir!!' falseChoice: 'Not so much...'  "	^self confirm: aString title: 'Please confirm:' trueChoice: trueChoice falseChoice: falseChoice at: nil! !!MorphicAlarmQueue class methodsFor: 'class initialization' stamp: 'jcg 1/9/2010 12:42'!convertAllAlarms	"Alarms should be kept in a MorphicAlarmQueue, not a Heap."	WorldState allSubInstancesDo: [:ws | ws convertAlarms]! !!WorldState methodsFor: 'alarms' stamp: 'jcg 1/9/2010 14:48'!removeAlarm: aSelector for: aTarget 	"Remove the alarm with the given selector"	self lockAlarmsDuring: [:locked |		| alarm |		alarm := locked 					detect: [:any | any receiver == aTarget and: [any selector == aSelector]]					ifNone: [nil].		alarm ifNotNil: [locked remove: alarm]	].! !!MorphicAlarmQueue methodsFor: 'comparing' stamp: 'jcg 1/9/2010 13:30'!sorts: alarmA before: alarmB	alarmA scheduledTime = alarmB scheduledTime 		ifFalse:[^alarmA scheduledTime < alarmB scheduledTime].	alarmA sequenceNumber = alarmB sequenceNumber		ifFalse:[^alarmA sequenceNumber < alarmB sequenceNumber].	^self error: 'These alarms run at the same time'! !!PluggableListMorph methodsFor: 'initialization' stamp: 'ar 1/23/2010 14:31'!getListElementSelector: aSymbol	"specify a selector that can be used to obtain a single element in the underlying list"	getListElementSelector == aSymbol ifTrue:[^self].	getListElementSelector := aSymbol.	list := nil.  "this cache will not be updated if getListElementSelector has been specified, so go ahead and remove it"! !!MorphicAlarmQueue class methodsFor: 'class initialization' stamp: 'jcg 1/9/2010 12:37'!initialize	self convertAllAlarms.! !!MorphicAlarmQueue methodsFor: 'private' stamp: 'jcg 1/9/2010 12:27'!isValidHeap	"Verify the correctness of the heap"	2 to: tally do:[:i|		(self sorts: (array at: i // 2) before: (array at: i)) ifFalse:[^false].	].	^true! !!MorphicAlarmQueue methodsFor: 'initialize' stamp: 'jcg 1/7/2010 10:53'!initialize	super initialize.	sequenceNumber := 0.! !!PolygonMorph methodsFor: 'private' stamp: 'nice 12/27/2009 22:39'!computeBounds	| oldBounds delta excludeHandles |	vertices ifNil: [^ self].	self changed.	oldBounds := bounds.	self releaseCachedState.	bounds := self curveBounds expanded copy.	self arrowForms do:		[:f | bounds swallow: (f offset extent: f extent)].	handles ifNotNil: [self updateHandles].	"since we are directly updating bounds, see if any ordinary submorphs exist and move them accordingly"	(oldBounds notNil and: [(delta := bounds origin - oldBounds origin) ~= (0@0)]) ifTrue: [		excludeHandles := IdentitySet new.		handles ifNotNil: [excludeHandles addAll: handles].		self submorphsDo: [ :each |			(excludeHandles includes: each) ifFalse: [				each position: each position + delta			].		].	].	self layoutChanged.	self changed.! !!MorphicProject methodsFor: 'futures' stamp: 'jcg 1/9/2010 13:12'!future: receiver send: aSelector at: deltaMSecs args: args	"Send a message deltaSeconds into the future.  Answers a Promise that will be resolved at some time in the future."	| pr closure |	pr := Promise new.	closure := [pr resolveWith: (receiver perform: aSelector withArguments: args)].	deltaMSecs = 0		ifTrue: [self addDeferredUIMessage: closure]		ifFalse: [			world 				addAlarm: #addDeferredUIMessage: 				withArguments: {closure}				for: self				at: (Time millisecondClockValue + deltaMSecs)		].	^pr		! !!TextContainer methodsFor: 'private' stamp: 'nice 12/27/2009 22:39'!bounds	| bounds theText |	self fillsOwner ifFalse: [^ textMorph textBounds].	theText := textMorph.	bounds := theText owner innerBounds.	bounds := bounds insetBy: (textMorph valueOfProperty: #margins ifAbsent: [1@1]).	theText owner submorphsBehind: theText do:		[:m | bounds swallow: m fullBounds].	^ bounds! !!WorldState methodsFor: 'alarms' stamp: 'jcg 1/9/2010 14:50'!adjustAlarmTimes: nowTime	"Adjust the alarm times after some clock weirdness (such as roll-over, image-startup etc)"	| deltaTime |	deltaTime := nowTime - lastAlarmTime.	self lockAlarmsDuring: [:locked |		locked do:[:alarm| alarm scheduledTime: alarm scheduledTime + deltaTime].	]! !!PasteUpMorph methodsFor: 'accessing' stamp: 'nk 10/13/2004 11:27'!lastKeystroke: aString	"Remember the last keystroke fielded by the receiver"	^ self setProperty: #lastKeystroke toValue: aString! !!WorldState methodsFor: 'stepping' stamp: 'jcg 1/9/2010 14:49'!cleanseStepListForWorld: aWorld	"Remove morphs from the step list that are not in this World.  Often were in a flap that has moved on to another world."	| deletions morphToStep |	deletions := nil.	stepList do: [:entry |		morphToStep := entry receiver.		morphToStep world == aWorld ifFalse:[			deletions ifNil: [deletions := OrderedCollection new].			deletions addLast: entry]].	deletions ifNotNil:[		deletions do: [:entry|			self stopStepping: entry receiver]].	self lockAlarmsDuring: [:locked |		locked copy do: [:entry |			morphToStep := entry receiver.			(morphToStep isMorph and:[morphToStep world == aWorld]) 				ifFalse:[self removeAlarm: entry selector for: entry receiver]]	].! !!PluggableTextMorph methodsFor: 'updating' stamp: 'ar 1/19/2010 20:31'!update: aSymbol with: arg1	aSymbol == #editString ifTrue:[		self editString: arg1.		self hasUnacceptedEdits: true.	].	^super update: aSymbol with: arg1! !!WorldState methodsFor: 'alarms' stamp: 'jcg 1/9/2010 14:46'!triggerAlarmsBefore: nowTime	"Trigger all pending alarms that are to be executed before nowTime."	lastAlarmTime ifNil:[lastAlarmTime := nowTime].	(nowTime < lastAlarmTime or:[nowTime - lastAlarmTime > 10000])		ifTrue:[self adjustAlarmTimes: nowTime].	self lockAlarmsDuring: [:pending |		[pending isEmpty not and:[pending first scheduledTime < nowTime]]			whileTrue:[pending removeFirst value: nowTime].	].	lastAlarmTime := nowTime.! !!WorldState methodsFor: 'object fileIn' stamp: 'jcg 1/9/2010 13:28'!convertAlarms	"We now store the alarms in a MorphicAlarmQueue, rather than a Heap."	alarms ifNotNil: [		alarms class == MorphicAlarmQueue ifFalse: [			| oldAlarms |			oldAlarms := alarms.			alarms := MorphicAlarmQueue new.			oldAlarms do: [:alarm | alarms add: alarm]					]	].! !!WorldState methodsFor: 'update cycle' stamp: 'al 7/31/2007 16:12'!interCyclePause: milliSecs	"delay enough that the previous cycle plus the amount of delay will equal milliSecs.  If the cycle is already expensive, then no delay occurs.  However, if the system is idly waiting for interaction from the user, the method will delay for a proportionally long time and cause the overall CPU usage of Squeak to be low.	If the preference #serverMode is enabled, always do a complete delay of 50ms, independant of my argument. This prevents the freezing problem described in Mantis #6581"	| currentTime wait |	Preferences serverMode		ifFalse: [			(lastCycleTime notNil and: [CanSurrenderToOS ~~ false]) ifTrue: [ 				currentTime := Time millisecondClockValue.				wait := lastCycleTime + milliSecs - currentTime.				(wait > 0 and: [ wait <= milliSecs ] ) ifTrue: [					(Delay forMilliseconds: wait) wait ] ] ]		ifTrue: [ (Delay forMilliseconds: 50) wait ].	lastCycleTime := Time millisecondClockValue.	CanSurrenderToOS := true.! !!WorldState methodsFor: 'alarms' stamp: 'jcg 1/9/2010 15:00'!lockAlarmsDuring: actionBlock	"All accesses to the alarms queue is synchronized by a mutex.  Answer the result of evaluating the 1-argument 'actionBlock'."	alarms ifNil: [alarms := MorphicAlarmQueue new].	^alarms mutex critical: [		actionBlock value: alarms	]! !!MorphicProject methodsFor: 'editors' stamp: 'dtl 1/24/2010 16:08'!formEdit: aForm	"Start up an instance of the form editor on a form." 	self inform: 'A Morphic editor has not been implemented. Enter an MVC project to edit this form.'! !!ProjectViewMorph methodsFor: 'initialization' stamp: 'ul 1/11/2010 07:27'!dismissViaHalo	| choice |	project ifNil:[^self delete]. "no current project"	choice := UIManager default chooseFrom: {		'yes - delete the window and the project' translated.		'no - delete the window only' translated	} title: ('Do you really want to delete {1}and all its content?' translated format: {project name printString}).	choice = 1 ifTrue:[^self expungeProject].	choice = 2 ifTrue:[^self delete].! !!TextEditor methodsFor: 'accessing-selection' stamp: 'ul 2/1/2010 01:15'!selectionAsStream	"Answer a ReadStream on the text in the paragraph that is currently 	selected."	^ReadWriteStream		on: paragraph string		from: self startIndex		to: self stopIndex - 1! !!WorldState methodsFor: 'alarms' stamp: 'jcg 1/9/2010 12:31'!alarms	^alarms ifNil: [alarms := MorphicAlarmQueue new]! !!PluggableTextMorph methodsFor: 'editor access' stamp: 'nice 1/10/2010 19:26'!handleEdit: editBlock	| result |	textMorph editor selectFrom: selectionInterval first to: selectionInterval last;						model: model.  "For, eg, evaluateSelection"	result := textMorph handleEdit: editBlock.   "Update selection after edit"	self scrollSelectionIntoView.	^ result! !!TextMorph methodsFor: 'editing' stamp: 'nice 1/10/2010 19:25'!handleEdit: editBlock	"Ensure that changed areas get suitably redrawn"	| result |	self selectionChanged.  "Note old selection"	result := editBlock value.	self selectionChanged.  "Note new selection"	self updateFromParagraph.  "Propagate changes as necessary"	^result! !!MorphicProject methodsFor: 'editors' stamp: 'dtl 1/24/2010 12:10'!editCharacter: character ofFont: strikeFont	"Open a bit editor on a character in the given strike font."	"Note that BitEditor only works in MVC currently."	"(TextStyle default fontAt: 1) edit: $="	self inform: 'A Morphic editor has not been implemented. Enter an MVC project to edit this font.'! !!WorldState methodsFor: 'alarms' stamp: 'jcg 1/9/2010 14:51'!addAlarm: aSelector withArguments: argArray for: aTarget at: scheduledTime	"Add a new alarm with the given set of parameters"	self lockAlarmsDuring: [:locked |		locked add:	(MorphicAlarm 						scheduledAt: scheduledTime						receiver: aTarget						selector: aSelector						arguments: argArray).	]! !!PasteUpMorph methodsFor: 'accessing' stamp: 'nk 10/13/2004 11:26'!lastKeystroke	"Answer the last keystroke fielded by the receiver"	^ self valueOfProperty: #lastKeystroke ifAbsent: ['']! !!MorphicAlarm methodsFor: 'accessing' stamp: 'jcg 1/7/2010 10:56'!sequenceNumber: positiveInteger	"Set the sequence number of the alarm, which is used to preserve ordering for alarms scheduled for the same time."	sequenceNumber := positiveInteger! !!PluggableTextMorph methodsFor: 'menu commands' stamp: 'nice 1/10/2010 19:27'!printIt	| result oldEditor |	textMorph editor selectFrom: selectionInterval first to: selectionInterval last;						model: model.  "For, eg, evaluateSelection"	result := textMorph handleEdit: [(oldEditor := textMorph editor) evaluateSelection].	((result isKindOf: FakeClassPool) or: [result == #failedDoit]) ifTrue: [^self flash].	selectionInterval := oldEditor selectionInterval.	textMorph installEditorToReplace: oldEditor.	textMorph handleEdit: [oldEditor afterSelectionInsertAndSelect: result printString].	selectionInterval := oldEditor selectionInterval.		textMorph editor selectFrom: selectionInterval first to: selectionInterval last.	self scrollSelectionIntoView.! !!ModalSystemWindowView methodsFor: 'controller access' stamp: 'dtl 1/24/2010 17:56'!defaultControllerClass	^Smalltalk at: #ModalController! !!MorphicProject methodsFor: 'editors' stamp: 'dtl 1/24/2010 16:09'!bitEdit: aForm at: magnifiedFormLocation scale: scaleFactor	"Create and schedule a view whose top left corner is magnifiedLocation 	and that contains a view of aForm magnified by scaleFactor that  can be	modified using the Bit Editor. It also contains a view of the original form."	self inform: 'A Morphic editor has not been implemented. Enter an MVC project to edit this form.'! !!MorphicProject methodsFor: 'editors' stamp: 'dtl 1/23/2010 18:26'!bitEdit: aForm	"Create and schedule a view located in an area designated by the user 	that contains a view of the receiver magnified by 8@8 that can be 	modified using the Bit Editor. It also contains a view of the original 	form."	aForm currentHand attachMorph: (FatBitsPaint new editForm: aForm;			 magnification: 8;			 brushColor: Color black;			 penSize: 1;			 yourself)! !!MorphicAlarmQueue methodsFor: 'accessing' stamp: 'jcg 1/9/2010 14:43'!mutex	^mutex ifNil: [mutex := Mutex new]! !!UserDialogBoxMorph class methodsFor: 'utilities' stamp: 'ar 1/23/2010 14:37'!confirm: aString title: titleString trueChoice: trueChoice falseChoice: falseChoice	"UserDialogBoxMorph confirm: 'Make your choice carefully' withCRs title: 'Do you like chocolate?' trueChoice: 'Oh yessir!!' falseChoice: 'Not so much...'  "	^self confirm: aString title: titleString trueChoice: trueChoice falseChoice: falseChoice at: nil! !!MorphicAlarm methodsFor: 'accessing' stamp: 'jcg 1/9/2010 12:29'!sequenceNumber	"Answer the sequence number of the alarm, which is used to preserve ordering for alarms scheduled for the same time."	^sequenceNumber ifNil: [0]! !!MorphicProject methodsFor: 'utilities' stamp: 'ar 1/10/2010 10:23'!offerMenu: menuSelector from: aModel shifted: aBoolean	"Pop up a menu whose target is aModel and whose contents are provided	by sending the menuSelector to the model. The menuSelector takes two	arguments: a menu, and a boolean representing the shift state."	| aMenu |	aMenu := MenuMorph new defaultTarget: aModel.	aModel perform: menuSelector with: aMenu with: aBoolean.	aMenu popUpInWorld! !!PasteUpMorph methodsFor: 'misc' stamp: 'ar 1/11/2010 19:55'!prepareToBeSaved	"Prepare for export via the ReferenceStream mechanism"	| exportDict soundKeyList players |	super prepareToBeSaved.	turtlePen := nil.	self isWorldMorph		ifTrue:			[self removeProperty: #scriptsToResume.			soundKeyList := Set new.			(players := self presenter allExtantPlayers)				do: [:aPlayer | aPlayer slotInfo						associationsDo: [:assoc | assoc value type == #Sound								ifTrue: [soundKeyList										add: (aPlayer instVarNamed: assoc key)]]].			players				do: [:p | p allScriptEditors						do: [:e | (e allMorphs								select: [:m | m isSoundTile])								do: [:aTile | soundKeyList add: aTile literal]]].			(self allMorphs				select: [:m | m isSoundTile])				do: [:aTile | soundKeyList add: aTile literal].			soundKeyList removeAllFoundIn: SampledSound universalSoundKeys.			soundKeyList				removeAllSuchThat: [:aKey | (SampledSound soundLibrary includesKey: aKey) not].			soundKeyList isEmpty				ifFalse: [exportDict := Dictionary new.					soundKeyList						do: [:aKey | exportDict								add: (SampledSound soundLibrary associationAt: aKey)].					self setProperty: #soundAdditions toValue: exportDict]]! !!JoystickMorph methodsFor: 'menu' stamp: 'nice 1/18/2010 18:58'!chooseJoystickNumber	"Allow the user to select a joystick number"	| result aNumber str |	str := self lastRealJoystickIndex asString.	result := UIManager default 				request: ('Joystick device number (currently {1})' translated format: {str})				initialAnswer: str.	aNumber := [result asNumber] on: Error do: [:err | ^Beeper beep].	(aNumber > 0 and: [aNumber <= 32]) 		ifFalse: 			["???"			^Beeper beep].	realJoystickIndex := aNumber.	self setProperty: #lastRealJoystickIndex toValue: aNumber.	self startStepping! !!PopUpMenu methodsFor: '*Morphic-Menus' stamp: 'dtl 1/30/2010 15:54'!morphicStartUpWithCaption: captionOrNil icon: aForm at: location allowKeyboard: aBoolean	"Display the menu, with caption if supplied. Wait for the mouse button to go down, then track the selection as long as the button is pressed. When the button is released,	Answer the index of the current selection, or zero if the mouse is not released over  any menu item. Location specifies the desired topLeft of the menu body rectangle. The final argument indicates whether the menu should seize the keyboard focus in order to allow the user to navigate it via the keyboard."	selection := Cursor normal				showWhile: [| menuMorph |					menuMorph := MVCMenuMorph from: self title: nil.					(captionOrNil notNil							or: [aForm notNil])						ifTrue: [menuMorph addTitle: captionOrNil icon: aForm].					MenuIcons decorateMenu: menuMorph.					menuMorph						invokeAt: location						in: ActiveWorld						allowKeyboard: aBoolean].	^ selection! !!SmalltalkEditor methodsFor: 'do-its' stamp: 'ul 2/1/2010 01:41'!tallySelection	"Treat the current selection as an expression; evaluate it and return the time took for this evaluation"	| result rcvr ctxt valueAsString v |	self lineSelectAndEmptyCheck: [^ -1].	(model respondsTo: #doItReceiver) 		ifTrue: [FakeClassPool adopt: model selectedClass.  "Include model pool vars if any"				rcvr := model doItReceiver.				ctxt := model doItContext]		ifFalse: [rcvr := ctxt := nil].	result := [ | cm |		cm := rcvr class evaluatorClass new 			compiledMethodFor: self selectionAsStream			in: ctxt			to: rcvr			notifying: self			ifFail: [FakeClassPool adopt: nil. ^ #failedDoit]			logged: false.		Time millisecondsToRun: 			[v := cm valueWithReceiver: rcvr arguments: #() ].	] 		on: OutOfScopeNotification 		do: [ :ex | ex resume: true].	FakeClassPool adopt: nil.	"We do not want to have large result displayed"	valueAsString := v printString.	(valueAsString size > 30) ifTrue: [valueAsString := (valueAsString copyFrom: 1 to: 30), '...'].	PopUpMenu 		inform: 'Time to compile and execute: ', result printString, 'ms res: ', valueAsString.! !Morph removeSelector: #isScriptEditorMorph!WorldState removeSelector: #alarmSortBlock!MorphicProject removeSelector: #offerMenuFrom:shifted:!MorphicAlarmQueue initialize!