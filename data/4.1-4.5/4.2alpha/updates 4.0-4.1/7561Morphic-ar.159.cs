"Change Set:		7561Morphic-ar.159Morphic-ar.159:Styling hooks:- Refactor the TextEditor>>changeEmphasis: logic  bit- Provide shout support methods in TextEditor- Add SmalltalkEditor>>styler ivar.Morphic-ar.156:Another round of reducing MVC/Morphic dependencies:- Remove the old openInMVC methods- Provide SystemWindow>>isWindowForModel:- Remove an indirect MVC dependency via window colorsMorphic-laza.157:I guess not too many use ProportionalSplitters to changethe layout of windows these days, but they are broken in thelatest trunk image. This is because the MethodcontainingWindow in Morph has changed. The containingWindow is now searched based on the model of thecomponent, but ProportionalSplitters do have no model.A quick fix is to look up the owner chain of aProportionalSplitter to find an owner that has a model. Most ofthe time (always?) the owner will be the containing window, soI don't know if the ifFalse: Branch is needed.Morphic-laza.158:Add check for owner chain without any of them having a model -> nil"!TextEditor subclass: #SmalltalkEditor	instanceVariableNames: 'styler'	classVariableNames: ''	poolDictionaries: ''	category: 'Morphic-Text Support'!!SystemWindow methodsFor: 'open/close' stamp: 'ar 8/15/2009 14:57'!openInWorldExtent: extent	"This msg and its callees result in the window being activeOnlyOnTop"	self openInWorld: self currentWorld extent: extent! !!SmalltalkEditor methodsFor: 'accessing' stamp: 'ar 8/18/2009 00:11'!styler: aStyler	"Sets the styler for this editor. Only code editors support syntax highlighting"	^styler := aStyler! !!TextEditor methodsFor: 'editing keys' stamp: 'ar 8/17/2009 23:27'!emphasisExtras	"Answer an array of extra items for the emphasis menu"	^#()! !!TextEditor methodsFor: 'parenblinking' stamp: 'ar 8/17/2009 23:46'!clearParens	"Clear parenthesis highlight"	lastParenLocation ifNotNil:		[self text string size >= lastParenLocation ifTrue: [			self text				removeAttribute: TextEmphasis bold				from: lastParenLocation				to: lastParenLocation]].	lastParenLocation := nil.! !!Morph methodsFor: 'e-toy support' stamp: 'laza 8/17/2009 01:06'!containingWindow	"Answer a window or window-with-mvc that contains the receiver"	| component |	component := self.	component model isNil ifTrue: [component := self firstOwnerSuchThat: [:m| m model notNil]].	^(component isNil or: [component isWindowForModel: component model])		ifTrue: [component]		ifFalse: [component firstOwnerSuchThat:[:m| m isWindowForModel: component model]]! !!Morph methodsFor: 'initialization' stamp: 'ar 8/15/2009 14:54'!openInWorld        "Add this morph to the world."      self openInWorld: self currentWorld.! !!TTSampleFontMorph methodsFor: 'initialize' stamp: 'ar 8/15/2009 14:54'!open	^self openInWorld! !!SmalltalkEditor methodsFor: 'editing keys' stamp: 'ar 8/17/2009 23:41'!handleEmphasisExtra: index with: characterStream	"Handle an extra emphasis menu item"	| action attribute thisSel oldAttributes |	action := {		[attribute := TextDoIt new.		thisSel := attribute analyze: self selection asString].		[attribute := TextPrintIt new.		thisSel := attribute analyze: self selection asString].		[attribute := TextLink new.		thisSel := attribute analyze: self selection asString with: 'Comment'].		[attribute := TextLink new.		thisSel := attribute analyze: self selection asString with: 'Definition'].		[attribute := TextLink new.		thisSel := attribute analyze: self selection asString with: 'Hierarchy'].		[attribute := TextLink new.		thisSel := attribute analyze: self selection asString].		[attribute := TextURL new.		thisSel := attribute analyze: self selection asString].		["Edit hidden info"		thisSel := self hiddenInfo.	"includes selection"		attribute := TextEmphasis normal].		["Copy hidden info"		self copyHiddenInfo.		^true].	"no other action"	} at: index.	action value.	thisSel ifNil: [^true].	"Could not figure out what to link to"	attribute ifNotNil: [		thisSel ifEmpty:[			"only change emphasisHere while typing"			oldAttributes := paragraph text attributesAt: self pointIndex.			self insertTypeAhead: characterStream.			emphasisHere _ Text addAttribute: attribute toArray: oldAttributes.		] ifNotEmpty: [			self replaceSelectionWith: (thisSel asText addAttribute: attribute).		]	].	^true! !!SmalltalkEditor methodsFor: 'accessing' stamp: 'ar 8/18/2009 00:11'!styler	"Answers the styler for this editor. Only code editors support syntax highlighting"	^styler! !!TTSampleFontMorph methodsFor: 'initialization' stamp: 'ar 8/15/2009 14:54'!openInWorld	HandMorph attach: self! !!SmalltalkEditor methodsFor: 'editing keys' stamp: 'ar 8/17/2009 23:43'!changeEmphasis: characterStream	"Change emphasis without styling if necessary"	| result |	styler ifNil:[^super changeEmphasis: characterStream].	styler evaluateWithoutStyling: [result := super changeEmphasis: characterStream].	^result! !!TextEditor methodsFor: 'accessing' stamp: 'ar 8/17/2009 23:56'!styler: aStyler	"Sets the styler for this editor. Only code editors support syntax highlighting"	^nil! !!TheWorldMenu methodsFor: 'construction' stamp: 'ar 8/15/2009 14:56'!appearanceMenu	"Build the appearance menu for the world."	| screenCtrl |	screenCtrl := ScreenController new.	^self fillIn: (self menu: 'appearance...') from: {		{'preferences...' . { self . #openPreferencesBrowser} . 'Opens a "Preferences Browser" which allows you to alter many settings' } .		{'choose theme...' . { Preferences . #offerThemesMenu} . 'Presents you with a menu of themes; each item''s balloon-help will tell you about the theme.  If you choose a theme, many different preferences that come along with that theme are set at the same time; you can subsequently change any settings by using a Preferences Panel'} .		nil .		{'system fonts...' . { self . #standardFontDo} . 'Choose the standard fonts to use for code, lists, menus, window titles, etc.'}.		{'text highlight color...' . { Preferences . #chooseTextHighlightColor} . 'Choose which color should be used for text highlighting in Morphic.'}.		{'insertion point color...' . { Preferences . #chooseInsertionPointColor} . 'Choose which color to use for the text insertion point in Morphic.'}.		{'keyboard focus color' . { Preferences . #chooseKeyboardFocusColor} . 'Choose which color to use for highlighting which pane has the keyboard focus'}.		nil.		{#menuColorString . { Preferences . #toggleMenuColorPolicy} . 'Governs whether menu colors should be derived from the desktop color.'}.		{#roundedCornersString . { Preferences . #toggleRoundedCorners} . 'Governs whether morphic windows and menus should have rounded corners.'}.		nil.		{'full screen on' . { screenCtrl . #fullScreenOn} . 'puts you in full-screen mode, if not already there.'}.		{'full screen off' . { screenCtrl . #fullScreenOff} . 'if in full-screen mode, takes you out of it.'}.		nil.		{'set display depth...' . {self. #setDisplayDepth} . 'choose how many bits per pixel.'}.		{'set desktop color...' . {self. #changeBackgroundColor} . 'choose a uniform color to use as desktop background.'}.		{'set gradient color...' . {self. #setGradientColor} . 'choose second color to use as gradient for desktop background.'}.		{'use texture background' . { #myWorld . #setStandardTexture} . 'apply a graph-paper-like texture background to the desktop.'}.		nil.		{'clear turtle trails from desktop' . { #myWorld . #clearTurtleTrails} . 'remove any pigment laid down on the desktop by objects moving with their pens down.'}.		{'pen-trail arrowhead size...' . { Preferences. #setArrowheads} . 'choose the shape to be used in arrowheads on pen trails.'}.	}! !!TextEditor methodsFor: 'parenblinking' stamp: 'ar 8/17/2009 23:46'!blinkParen	"Highlight the last parenthesis in the text"	lastParenLocation ifNotNil:		[self text string size >= lastParenLocation ifTrue: [			self text				addAttribute: TextEmphasis bold				from: lastParenLocation				to: lastParenLocation]]! !!SystemWindow methodsFor: 'testing' stamp: 'ar 8/15/2009 15:05'!isWindowForModel: aModel	"Return true if the receiver acts as the window for the given model"	^aModel == self model! !!TextEditor methodsFor: 'editing keys' stamp: 'ar 8/17/2009 23:39'!handleEmphasisExtra: index with: characterStream	"Handle an emphasis extra choice"	^true! !!TextEditor methodsFor: 'accessing' stamp: 'ar 8/17/2009 23:56'!styler	"Answers the styler for this editor. Only code editors support syntax highlighting"	^nil! !!SmalltalkEditor methodsFor: 'editing keys' stamp: 'ar 8/17/2009 23:28'!emphasisExtras	^#(		'Do it' 		'Print it' 		'Link to comment of class' 		'Link to definition of class' 		'Link to hierarchy of class' 		'Link to method'	).! !!TextEditor methodsFor: 'editing keys' stamp: 'ar 8/17/2009 23:40'!changeEmphasis: characterStream 	"Change the emphasis of the current selection or prepare to accept characters with the change in emphasis. Emphasis change amounts to a font change.  Keeps typeahead."	"control 0..9 -> 0..9"	| keyCode attribute oldAttributes index thisSel colors extras |	keyCode := ('0123456789-=' indexOf: sensor keyboard ifAbsent: [1]) - 1.	oldAttributes := paragraph text attributesAt: self pointIndex.	thisSel := self selection.	"Decipher keyCodes for Command 0-9..."	(keyCode between: 1 and: 5) 		ifTrue: [attribute := TextFontChange fontNumber: keyCode].	keyCode = 6 		ifTrue: [			colors := #(#black #magenta #red #yellow #green #blue #cyan #white).			extras := self emphasisExtras.			index := UIManager default chooseFrom:colors , #('choose color...' ), extras						lines: (Array with: colors size + 1).			index = 0 ifTrue: [^true].			index <= colors size 				ifTrue: [attribute := TextColor color: (Color perform: (colors at: index))]				ifFalse: [					index := index - colors size - 1.	"Re-number!!!!!!"					index = 0 						ifTrue: [attribute := self chooseColor]						ifFalse:[^self handleEmphasisExtra: index with: characterStream]	"handle an extra"]].	(keyCode between: 7 and: 11) 		ifTrue: [			sensor leftShiftDown 				ifTrue: [					keyCode = 10 ifTrue: [attribute := TextKern kern: -1].					keyCode = 11 ifTrue: [attribute := TextKern kern: 1]]				ifFalse: [					attribute := TextEmphasis 								perform: (#(#bold #italic #narrow #underlined #struckOut) at: keyCode - 6).					oldAttributes 						do: [:att | (att dominates: attribute) ifTrue: [attribute turnOff]]]].	keyCode = 0 ifTrue: [attribute := TextEmphasis normal].	attribute ifNotNil: [		thisSel size = 0			ifTrue: [				"only change emphasisHere while typing"				self insertTypeAhead: characterStream.				emphasisHere _ Text addAttribute: attribute toArray: oldAttributes ]			ifFalse: [				self replaceSelectionWith: (thisSel asText addAttribute: attribute) ]].	^true! !Morph removeSelector: #openInMVC!PasteUpMorph removeSelector: #openWithTitle:cautionOnClose:!PasteUpMorph removeSelector: #open!SystemWindow removeSelector: #openInMVC!SystemWindow removeSelector: #openInMVCExtent:!