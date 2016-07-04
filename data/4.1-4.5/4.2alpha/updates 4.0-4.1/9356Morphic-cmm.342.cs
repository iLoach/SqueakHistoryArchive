"Change Set:		9356Morphic-cmm.342Morphic-cmm.342:- Added four useful hot-keys to SmalltalkEditor.  Cmd+1 - Cmd+4 will paste that method argument into the method.Rationale:  Productivity.  When typing method source, you almost always need to utilize the incoming arguments, it is therefore very convenient to be able to do so with one hot-key.Morphic-cmm.338:- Filed-in Andreas' fix for when an OrderedCollection inspector's scroll-bar is not properly set on initial open.- 3.11 look-change; 'halos' both by their name and their function, should be more like a wisp than a solid object.  Therefore, their borderWidth has been changed to 0.Morphic-cmm.339:Added 'Destructive Back Word' preference.  It defaults to the worse, legacy option, true.  Try setting it false and note how nice a non-destructive back-word command is:	- Ability to select the prior text while keeping hands in the typing position.	- When used in conjunction with SelectionsMayShrink=false, it becomes useful in the debugger for selecting and evaluating expressions using gross (vs. fine) motor-skill, preserving developer energy.	- The existing behavior, replacing the prior word(s) with something else is preserved verbatim, no additional gestures.	- The existing behavior, to delete prior words (without replacing them with anything) does now require the deliberate action of delete (or backspace).  However, this is actually better than destructive backWord because the selection lets you confirm what you want to delete, instead of accidently deleting more than you want (i.e., when punctuation is involved).Morphic-dtl.340:Remove explicit reference to MVC Switch from PluggableButtonMorph class>>example.Implement MorphicProject>>showImage:named: to eliminate MVC/Morphic dependency in HTTPSocket.Morphic-nice.341:remove fixTempsmove a temp declaration inside blocks"!Object subclass: #Editor	instanceVariableNames: 'sensor morph selectionShowing'	classVariableNames: 'DestructiveBackWord SelectionsMayShrink'	poolDictionaries: ''	category: 'Morphic-Text Support'!!Editor class methodsFor: 'preferences' stamp: 'cmm 2/12/2010 15:56'!destructiveBackWord	<preference: 'Destructive Back-Word'		category: 'Morphic'		description: 'Indicates whether the back-word command deletes, or merely selects, the prior word.'		type: #Boolean>	^ DestructiveBackWord ifNil: [ true ]! !!MorphicProject methodsFor: 'file in/out' stamp: 'nice 2/9/2010 15:17'!armsLengthCommand: aCommand withDescription: aString	| tempProject foolingForm tempCanvas bbox crossHatchColor stride |	"Set things up so that this aCommand is sent to self as a messageafter jumping to the parentProject.  For things that can't be executedwhile in this project, such as saveAs, loadFromServer, storeOnServer.  SeeProjectViewMorph step."	world borderWidth: 0.	"get rid of the silly default border"	tempProject := MorphicProject new.	foolingForm := world imageForm.		"make them think they never left"	tempCanvas := foolingForm getCanvas.	bbox := foolingForm boundingBox.	crossHatchColor := Color yellow alpha: 0.3.	stride := 20.	10 to: bbox width by: stride do: [ :x |		tempCanvas fillRectangle: (x@0 extent: 1@bbox height) fillStyle: crossHatchColor.	].	10 to: bbox height by: stride do: [ :y |		tempCanvas fillRectangle: (0@y extent: bbox width@1) fillStyle: crossHatchColor.	].	tempProject world color: (InfiniteForm with: foolingForm).	tempProject projectParameters 		at: #armsLengthCmd 		put: (			DoCommandOnceMorph new				addText: aString;				actionBlock: [					self doArmsLengthCommand: aCommand.				]		).	tempProject projectParameters 		at: #deleteWhenEnteringNewProject 		put: true.	tempProject enter! !!LazyListMorph methodsFor: 'list management' stamp: 'ar 5/4/2007 22:48'!listChanged	"set newList to be the list of strings to display"	listItems := Array new: self getListSize withAll: nil.	maxWidth := nil.	selectedRow := nil.	selectedRows := PluggableSet integerSet.	self adjustHeight.	self adjustWidth.	self changed.! !!Editor class methodsFor: 'preferences' stamp: 'cmm 2/12/2010 15:56'!selectionsMayShrink: aBoolean	SelectionsMayShrink := aBoolean! !!SmalltalkEditor methodsFor: 'private' stamp: 'cmm 2/13/2010 14:42'!methodArgument: anInteger	^ (ReadStream on: self text asString) nextLine substrings 		at: 2*anInteger		ifAbsent: [ String empty ]! !!Editor methodsFor: 'typing/selecting keys' stamp: 'cmm 2/12/2010 15:58'!nonDestructiveBackWord: characterStream 	"Select the prior word."	| indices newPosition |	self closeTypeIn: characterStream.	indices := self 		setIndices: true		forward: false.	newPosition := 1 max: (indices at: #moving)-1.	newPosition :=  self previousWord: newPosition.	sensor keyboard.	self selectMark: (indices at: #fixed) point: newPosition - 1.	^ true! !!Editor class methodsFor: 'preferences' stamp: 'cmm 2/12/2010 15:57'!destructiveBackWord: aBoolean	DestructiveBackWord := aBoolean! !!SmalltalkEditor methodsFor: 'private' stamp: 'cmm 2/13/2010 14:44'!typeMethodArgument: characterStream 	"Replace the current text selection with the name of the method argument represented by the keyCode."	| keyCode |	keyCode := ('1234' 		indexOf: sensor keyboard 		ifAbsent: [1]).	characterStream nextPutAll: (self methodArgument: keyCode).	^ false! !!Editor methodsFor: 'typing/selecting keys' stamp: 'cmm 2/12/2010 15:58'!destructiveBackWord: characterStream 	"If the selection is not a caret, delete it and leave it in the backspace buffer.	 Else if there is typeahead, delete it.	 Else, delete the word before the caret."	| startIndex |	sensor keyboard.	characterStream isEmpty		ifTrue:			[self hasCaret				ifTrue: "a caret, delete at least one character"					[startIndex := 1 max: self markIndex - 1.					[startIndex > 1 and:						[(self string at: startIndex - 1) tokenish]]						whileTrue:							[startIndex := startIndex - 1]]				ifFalse: "a non-caret, just delete it"					[startIndex := self markIndex].			self backTo: startIndex]		ifFalse:			[characterStream reset].	^false! !!PolygonMorph methodsFor: 'editing' stamp: 'nice 2/8/2010 08:45'!updateHandles	self isCurvy		ifTrue: [handles first center: vertices first.			handles last center: vertices last.			self midVertices				withIndexDo: [:midPt :vertIndex | (closed							or: [vertIndex < vertices size])						ifTrue: [| newVert |							newVert := handles atWrap: vertIndex * 2.							newVert position: midPt - (newVert extent // 2)]]]		ifFalse: [vertices				withIndexDo: [:vertPt :vertIndex |					| oldVert | 					oldVert := handles at: vertIndex * 2 - 1.					oldVert position: vertPt - (oldVert extent // 2).					(closed							or: [vertIndex < vertices size])						ifTrue: [| newVert |							newVert := handles at: vertIndex * 2.							newVert position: vertPt									+ (vertices atWrap: vertIndex + 1) - newVert extent // 2 + (1 @ -1)]]]! !!PluggableButtonMorph class methodsFor: 'example' stamp: 'dtl 2/12/2010 19:17'!example	"PluggableButtonMorph example openInWorld"	| s1 s2 s3 b1 b2 b3 row switchClass |	switchClass := Smalltalk at: #Switch ifAbsent: [^self inform: 'MVC class Switch not present'].	s1 := switchClass new.	s2 := switchClass new turnOn.	s3 := switchClass new.	s2 onAction: [s3 turnOff].	s3 onAction: [s2 turnOff].	b1 := (PluggableButtonMorph on: s1 getState: #isOn action: #switch) label: 'S1'.	b2 := (PluggableButtonMorph on: s2 getState: #isOn action: #turnOn) label: 'S2'.	b3 := (PluggableButtonMorph on: s3 getState: #isOn action: #turnOn) label: 'S3'.	b1		hResizing: #spaceFill;		vResizing: #spaceFill.	b2		hResizing: #spaceFill;		vResizing: #spaceFill.	b3		hResizing: #spaceFill;		vResizing: #spaceFill.	row := AlignmentMorph newRow		hResizing: #spaceFill;		vResizing: #spaceFill;		addAllMorphs: (Array with: b1 with: b2 with: b3);		extent: 120@35.	^ row! !!SmalltalkEditor class methodsFor: 'keyboard shortcut tables' stamp: 'cmm 2/13/2010 14:47'!initializeCmdKeyShortcuts	"Initialize the (unshifted) command-key (or alt-key) shortcut table."	"NOTE: if you don't know what your keyboard generates, use Sensor kbdTest"	"SmalltalkEditor initialize"	| cmds |	super initializeCmdKeyShortcuts.	cmds := #($b #browseIt: $d #doIt: $i #inspectIt: $j #doAgainOnce: $l #cancel: $m #implementorsOfIt: $n #sendersOfIt: $o #spawnIt: $p #printIt: $q #querySymbol: $s #save: ).	1 to: cmds size		by: 2		do: [ : i | cmdActions at: (cmds at: i) asciiValue + 1 put: (cmds at: i + 1)].	"Set up type-method argument hot keys, 1-4.."	'1234' do:		[ : eachKeyboardChar |		cmdActions 			at: eachKeyboardChar asciiValue + 1			put: #typeMethodArgument: ]! !!HaloMorph methodsFor: 'private' stamp: 'cmm 2/9/2010 18:12'!createHandleAt: aPoint color: aColor iconName: iconName 	| bou handle |	bou := Rectangle center: aPoint extent: self handleSize asPoint.	Preferences alternateHandlesLook		ifTrue: [			handle := RectangleMorph newBounds: bou color: aColor.			handle borderWidth: 0.			handle useRoundedCorners.			self setColor: aColor toHandle: handle]		ifFalse: [handle := EllipseMorph newBounds: bou color: aColor].	""	handle borderColor: aColor muchDarker.	handle wantsYellowButtonMenu: false.	""	iconName isNil		ifFalse: [| form | 			form := ScriptingSystem formAtKey: iconName.			form isNil				ifFalse: [| image | 					image := ImageMorph new.					image image: form.					image color: aColor makeForegroundColor.					image lock.					handle addMorphCentered: image]].	""	^ handle! !!MorphicProject methodsFor: 'utilities' stamp: 'dtl 2/12/2010 20:41'!showImage: aForm named: imageName	"Show an image, possibly attached to the pointer for positioning"	HandMorph attach: (World drawingClass withForm: aForm)! !!Editor methodsFor: 'typing/selecting keys' stamp: 'cmm 2/12/2010 16:00'!backWord: characterStream 	^ self class destructiveBackWord 		ifTrue: [ self destructiveBackWord: characterStream ]		ifFalse: [ self nonDestructiveBackWord: characterStream ]! !!SelectionMorph methodsFor: 'dropping/grabbing' stamp: 'nice 2/9/2010 15:17'!justDroppedInto: newOwner event: evt	selectedItems isEmpty ifTrue:		["Hand just clicked down to draw out a new selection"		^ self extendByHand: evt hand].	dupLoc ifNotNil: [dupDelta := self position - dupLoc].	selectedItems reverseDo: [:m | 		WorldState addDeferredUIMessage:			[m referencePosition: (newOwner localPointToGlobal: m referencePosition).			newOwner handleDropMorph:				(DropEvent new setPosition: evt cursorPoint contents: m hand: evt hand)]].	evt wasHandled: true! !