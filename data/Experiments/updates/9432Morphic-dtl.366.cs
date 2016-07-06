'From Squeak3.11alpha of 13 February 2010 [latest update: #9483] on 9 March 2010 at 11:11:23 am'!ScrollPane subclass: #PluggableTextMorph	instanceVariableNames: 'textMorph getTextSelector setTextSelector getSelectionSelector hasUnacceptedEdits askBeforeDiscardingEdits selectionInterval hasEditingConflicts '	classVariableNames: 'SimpleFrameAdornments AdornmentCache '	poolDictionaries: ''	category: 'Morphic-Pluggable Widgets'!!DockingBarItemMorph methodsFor: 'selecting' stamp: 'kb 2/26/2010 14:02'!select: evt		super select: evt.	subMenu ifNotNil: [		evt hand newKeyboardFocus: subMenu ]! !!DockingBarMorph methodsFor: 'events-processing' stamp: 'kb 2/26/2010 13:07'!handleListenEvent: anEvent	" I am registered as a keyboardListener of the ActiveHand, 	watching for ctrl-<n> keystrokes, and upon them if I have 	an nth menu item, I'll activate myself and select it. "		(anEvent controlKeyPressed and: [ 		anEvent keyValue 			between: 49 " $1 asciiValue " 			and: 57 " $9 asciiValue " ]) ifTrue: [ 		| index itemToSelect |		index := anEvent keyValue - 48.		itemToSelect := (submorphs select: [ :each | 			each isKindOf: DockingBarItemMorph ]) at: index ifAbsent: [ ^self ].		self activate: anEvent.		self 			selectItem: itemToSelect			event: anEvent ]! !!DockingBarMorph methodsFor: 'events-processing' stamp: 'kb 2/26/2010 14:02'!keyStroke: evt 	| asc |	asc := evt keyCharacter asciiValue.	asc = 27 ifTrue: [ "escape key" 		^self deactivate: evt ].	asc = self selectSubmenuKey ifTrue: [		self ensureSelectedItem: evt.		self selectedItem subMenu ifNotNil: [ :subMenu |			subMenu items ifNotEmpty: [				subMenu activate: evt.				^subMenu moveSelectionDown: 1 event: evt ] ] ].	asc = self previousKey ifTrue: [ ^self moveSelectionDown: -1 event: evt ].	asc = self nextKey ifTrue: [ ^self moveSelectionDown: 1 event: evt ].	selectedItem ifNotNil: [ 		selectedItem subMenu ifNotNil: [ :subMenu |			" If we didn't handle the keystroke, pass the keyboard focus 			to the open submenu. "			evt hand newKeyboardFocus: subMenu.			subMenu keyStroke: evt ] ]! !!MorphicProject methodsFor: 'display' stamp: 'dtl 2/27/2010 10:34'!resetDisplay 	"Bring the display to a usable state after handling primitiveError."	World install "init hands and redisplay"! !!MorphicProject methodsFor: 'utilities' stamp: 'dtl 2/27/2010 10:53'!handleFatalDrawingError: errMsg	"Handle a fatal drawing error."	Display deferUpdates: false. "Just in case"	self primitiveError: errMsg	"Hm... we should jump into a 'safe' worldState here, but how do we find it?!!"! !!MorphicProject methodsFor: 'utilities' stamp: 'dtl 2/27/2010 09:55'!setAsBackground: aForm	"Set  aForm as a background image."	| world newColor |	world := self currentWorld.	newColor := InfiniteForm with: aForm.	aForm rememberCommand:		(Command new cmdWording: 'set background to a picture' translated;			undoTarget: world selector: #color: argument: world color;			redoTarget: world selector: #color: argument: newColor).	world color: newColor! !!PasteUpMorph methodsFor: 'world state' stamp: 'dtl 2/27/2010 10:58'!handleFatalDrawingError: errMsg	"Handle a fatal drawing error."	self flag: #toRemove. "Implementation moved to Project, but are there external packages with senders?"	Project current handleFatalDrawingError: errMsg! !!PluggableTextMorph methodsFor: 'drawing' stamp: 'ar 3/1/2010 19:41'!drawFrameAdornment: aColor on: aCanvas	"Indicate edit status for the text editor"	| form |	self class simpleFrameAdornments		ifTrue:[^aCanvas frameRectangle: self innerBounds width: 2 color: aColor].	"Class-side adornment cache is currently using pre-multiplied alpha, 	so we need to use rule 34 which works for < 32bpp, too."	form := self class adornmentWithColor: aColor.	aCanvas image: form at: (self innerBounds topRight - (form width@0))		sourceRect: form boundingBox rule: 34.! !!PluggableTextMorph methodsFor: 'drawing' stamp: 'ar 2/26/2010 17:02'!drawFrameAdornmentsOn: aCanvas 	"Include a thin red inset border for unaccepted edits, or, if the unaccepted edits are known to conflict with a change made somewhere else to the same method (typically), put a thick red frame"	self wantsFrameAdornments ifTrue:		[(model notNil and: [model refusesToAcceptCode])			ifTrue:  "Put up feedback showing that code cannot be submitted in this state"				[self drawFrameAdornment: Color tan on: aCanvas]			ifFalse:				[self hasEditingConflicts					ifTrue:						[self drawFrameAdornment: Color red on: aCanvas] 					ifFalse:						[self hasUnacceptedEdits							ifTrue:								[model wantsDiffFeedback									ifTrue:										[self drawFrameAdornment: Color yellow on: aCanvas]									ifFalse:										[self drawFrameAdornment: Color orange on: aCanvas]]							ifFalse:								[model wantsDiffFeedback									ifTrue:										[self drawFrameAdornment: Color green on: aCanvas]]]]]! !!PluggableTextMorph methodsFor: 'drawing' stamp: 'ar 2/26/2010 17:06'!fullDrawOn: aCanvas 	"Draw frame adornments on top of everything otherwise they will partially overlap with text selection which looks ugly."	super fullDrawOn: aCanvas. 	self drawFrameAdornmentsOn: aCanvas.! !!PluggableTextMorph class methodsFor: 'frame adornments' stamp: 'ar 2/26/2010 16:23'!adornmentCache	"Cache for frame adornments"	^AdornmentCache ifNil:[AdornmentCache := Dictionary new].! !!PluggableTextMorph class methodsFor: 'frame adornments' stamp: 'ar 2/26/2010 16:33'!adornmentWithColor: aColor	"Create and return a frame adornment with the given color"	| size box form fillStyle |	^self adornmentCache at: aColor ifAbsentPut:[		size := 25. 		box := 0@0 extent: size asPoint.		form := Form extent: size@size depth: 32.		fillStyle := (GradientFillStyle ramp: {			0.0->(Color white alpha: 0.01).			0.8->aColor.			1.0->aColor})			origin: box topRight - (size@0);			direction: (size @ size negated) // 4;			radial: false.		form getCanvas drawPolygon:  {			box topRight. 			box topRight + (0@size). 			box topRight - (size@0)		} fillStyle: fillStyle.		form].! !!PluggableTextMorph class methodsFor: 'frame adornments' stamp: 'ar 2/26/2010 16:23'!flushAdornmentCache	"Cache for frame adornments"	AdornmentCache := nil! !!ScrollBar class methodsFor: 'class initialization' stamp: 'ar 2/26/2010 23:05'!cleanUp	"Re-initialize the image cache"	self initializeImagesCache! !!StandardScriptingSystem class methodsFor: 'class initialization' stamp: 'ar 2/27/2010 00:19'!cleanUp: agressive	"Clean up unreferenced players. If agressive, reinitialize and nuke players"	self removeUnreferencedPlayers.	agressive ifTrue:[		References keys do: [:k | References removeKey: k].		self initialize.	].! !!SystemWindow methodsFor: 'initialization' stamp: 'laza 2/26/2010 20:42'!addCloseBox	"If I have a labelArea, add a close box to it"	| frame |	labelArea		ifNil: [^ self].	closeBox := self createCloseBox.	frame := LayoutFrame new.	frame leftFraction: 0;		 leftOffset: 2;		 topFraction: 0;		 topOffset: 0.	closeBox layoutFrame: frame.	labelArea addMorphFront: closeBox! !!SystemWindow methodsFor: 'menu' stamp: 'laza 2/26/2010 22:06'!buildWindowMenu	| aMenu |	aMenu := MenuMorph new defaultTarget: self.	aMenu add: 'change title...' translated action: #relabel.	aMenu addLine.	aMenu add: 'send to back' translated action: #sendToBack.	aMenu add: 'make next-to-topmost' translated action: #makeSecondTopmost.	aMenu addLine.	self mustNotClose		ifFalse:			[aMenu add: 'make unclosable' translated action: #makeUnclosable]		ifTrue:			[aMenu add: 'make closable' translated action: #makeClosable].	aMenu		add: (self isSticky ifTrue: ['make draggable'] ifFalse: ['make undraggable']) translated 		action: #toggleStickiness.	aMenu addLine.	self unexpandedFrame 		ifNil: [aMenu add: 'full screen' translated action: #expandBoxHit]		ifNotNil: [aMenu add: 'original size' translated action: #expandBoxHit].	self isCollapsed ifFalse: [aMenu add: 'window color...' translated action: #setWindowColor].	^aMenu! !!SystemWindow methodsFor: 'menu' stamp: 'laza 2/26/2010 20:41'!makeClosable	mustNotClose := false.	closeBox ifNil: [self addCloseBox]! !!SystemWindow methodsFor: 'resize/collapse' stamp: 'laza 2/26/2010 21:56'!contractToOriginalSize	self bounds: self unexpandedFrame.	self unexpandedFrame: nil.	^ expandBox setBalloonText: 'expand to full screen' translated! !!SystemWindow methodsFor: 'resize/collapse' stamp: 'laza 2/26/2010 21:58'!expandBoxHit	isCollapsed		ifTrue: [self	hide;					collapseOrExpand;					expandToFullScreen;					show]		ifFalse: [self unexpandedFrame 					ifNil: [self expandToFullScreen]					ifNotNil: [self contractToOriginalSize]]! !!SystemWindow methodsFor: 'resize/collapse' stamp: 'laza 2/26/2010 21:53'!expandToFullScreen	self unexpandedFrame ifNil: [ self unexpandedFrame: fullFrame ].	self fullScreen.	expandBox setBalloonText: 'contract to original size' translated! !!SystemWindow methodsFor: 'top window' stamp: 'laza 2/26/2010 16:57'!activateWindow	"Bring me to the front and make me able to respond to mouse and keyboard.	Was #activate (sw 5/18/2001 23:20)"	| oldTop outerMorph sketchEditor pal |	outerMorph := self topRendererOrSelf.	outerMorph owner ifNil: [^ self "avoid spurious activate when drop in trash"].	oldTop := TopWindow.	oldTop = self ifTrue: [^self].	TopWindow := self.	oldTop ifNotNil: [oldTop passivate].	outerMorph owner firstSubmorph == outerMorph		ifFalse: ["Bring me (with any flex) to the top if not already"				outerMorph owner addMorphFront: outerMorph].	self submorphsDo: [:m | m unlock].	label ifNotNil: [label color: Color black].	labelArea ifNotNil:		[labelArea submorphsDo: [:m | m unlock; show].		self setStripeColorsFrom: self paneColorToUse].	self isCollapsed ifFalse:		[model modelWakeUpIn: self.		self positionSubmorphs.		labelArea ifNil: [self adjustBorderUponActivationWhenLabeless]].	(sketchEditor := self extantSketchEditor) ifNotNil:		[sketchEditor comeToFront.		(pal := self world findA: PaintBoxMorph) ifNotNil:			[pal comeToFront]].! !!SystemWindow methodsFor: 'top window' stamp: 'laza 2/26/2010 17:40'!passivate	"Make me unable to respond to mouse and keyboard"	label ifNotNil: [label color: Color darkGray].	self setStripeColorsFrom: self paneColorToUse.	model modelSleep.	"Control boxes remain active, except in novice mode"	self submorphsDo: [:m |		m == labelArea ifFalse:			[m lock]].	labelArea ifNotNil:		[labelArea submorphsDo: [:m |			m == label				ifTrue: [m lock]				ifFalse: [					Preferences noviceMode						ifTrue: [m lock; hide]						ifFalse: [							(m == closeBox or: [m == collapseBox])								ifFalse: [m lock; hide]]]]]		ifNil: "i.e. label area is nil, so we're titleless"			[self adjustBorderUponDeactivationWhenLabeless].! !!TextEditor methodsFor: 'events' stamp: 'kb 2/26/2010 15:49'!mouseDown: evt 	"An attempt to break up the old processRedButton code into threee phases"	| clickPoint b |	oldInterval := self selectionInterval.	clickPoint := evt cursorPoint.	b := paragraph characterBlockAtPoint: clickPoint.	(paragraph clickAt: clickPoint for: model controller: self) ifTrue: [		self markBlock: b.		self pointBlock: b.		evt hand releaseKeyboardFocus: self.		^ self ].		evt shiftPressed		ifFalse: [			self closeTypeIn.			self markBlock: b.			self pointBlock: b ]		 ifTrue: [			self closeTypeIn.			self mouseMove: evt ].       self storeSelectionInParagraph! !!TheWorldMenu class methodsFor: 'class initialization' stamp: 'ar 2/26/2010 23:06'!cleanUp	"Flush out obsolete entries"	self removeObsolete! !!WorldState methodsFor: 'update cycle' stamp: 'dtl 2/27/2010 10:57'!displayWorldSafely: aWorld	"Update this world's display and keep track of errors during draw methods."	[aWorld displayWorld] ifError: [:err :rcvr |		"Handle a drawing error"		| errCtx errMorph |		errCtx := thisContext.		[			errCtx := errCtx sender.			"Search the sender chain to find the morph causing the problem"			[errCtx notNil and:[(errCtx receiver isMorph) not]] 				whileTrue:[errCtx := errCtx sender].			"If we're at the root of the context chain then we have a fatal drawing problem"			errCtx ifNil:[^Project current handleFatalDrawingError: err].			errMorph := errCtx receiver.			"If the morph causing the problem has already the #drawError flag set,			then search for the next morph above in the caller chain."			errMorph hasProperty: #errorOnDraw		] whileTrue.		errMorph setProperty: #errorOnDraw toValue: true.		"Install the old error handler, so we can re-raise the error"		rcvr error: err.	].! !!WorldState methodsFor: 'update cycle' stamp: 'dtl 2/27/2010 10:58'!handleFatalDrawingError: errMsg	"Handle a fatal drawing error."	self flag: #toRemove. "Implementation moved to Project, but are there external packages with senders?"	Project current handleFatalDrawingError: errMsg! !!WorldState class methodsFor: 'class initialization' stamp: 'ar 2/26/2010 23:06'!cleanUp	"Reset command histories"	self allInstances do: [ :ea | ea clearCommandHistory ].! !PluggableTextMorph removeSelector: #drawOn:!ScrollPane subclass: #PluggableTextMorph	instanceVariableNames: 'textMorph getTextSelector setTextSelector getSelectionSelector hasUnacceptedEdits askBeforeDiscardingEdits selectionInterval hasEditingConflicts'	classVariableNames: 'AdornmentCache SimpleFrameAdornments'	poolDictionaries: ''	category: 'Morphic-Pluggable Widgets'!