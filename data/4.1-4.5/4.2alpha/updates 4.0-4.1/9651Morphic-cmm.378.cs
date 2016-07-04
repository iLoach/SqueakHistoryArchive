"Change Set:		9651Morphic-cmm.378Morphic-cmm.378:- Fixed bug with MenuMorph>>#addStayUpIcons.- Utilities class>>#authorInitialsPerSe is often set to an empty string, not nil.  Updated some guards accordingly.- The changed-indicator was made two-pixels thick from 3.10.  Instead of putting it back to one, I softened it a bit, which looks better and more refined."!!PluggableTextMorph methodsFor: 'drawing' stamp: 'cmm 3/8/2010 14:37'!drawFrameAdornment: aColor on: aCanvas 	"Indicate edit status for the text editor"	self class simpleFrameAdornments		ifTrue:			[ aCanvas				frameRectangle: self innerBounds				width: 1				color: aColor.			aCanvas				frameRectangle: (self innerBounds insetBy: 1)				width: 1				color: (aColor alpha: aColor alpha / 3.0) ]		ifFalse:			[ | form |			"Class-side adornment cache is currently using pre-multiplied alpha, so we need to use rule 34 which works for < 32bpp, too."			form := self class adornmentWithColor: aColor.			aCanvas				image: form				at: self innerBounds topRight - (form width @ 0)				sourceRect: form boundingBox				rule: 34 ]! !!Morph methodsFor: 'fileIn/out' stamp: 'cmm 3/9/2010 15:12'!saveOnURL: suggestedUrlString 	"Save myself on a SmartReferenceStream file.  If I don't already have a url, use the suggested one.  Writes out the version and class structure.  The file is fileIn-able.  UniClasses will be filed out."	| url pg stamp pol |	(pg := self valueOfProperty: #SqueakPage)		ifNil: [ pg := SqueakPage new ]		ifNotNil:			[ pg contentsMorph ~~ self ifTrue:				[ self inform: 'morph''s SqueakPage property is out of date'.				pg := SqueakPage new ] ].	(url := pg url) ifNil: [ url := pg urlNoOverwrite: suggestedUrlString ].	stamp := Utilities authorInitialsPerSe.	stamp isEmptyOrNil ifTrue: [ stamp := '*' ].	pg		saveMorph: self		author: stamp.	SqueakPageCache		atURL: url		put: pg.	"setProperty: #SqueakPage"	(pol := pg policy) ifNil: [ pol := #neverWrite ].	pg		 policy: #now ;		 dirty: true.	pg write.	"force the write"	pg policy: pol.	^pg! !!Morph methodsFor: 'fileIn/out' stamp: 'cmm 3/9/2010 15:13'!saveOnURLbasic	"Ask the user for a url and save myself on a SmartReferenceStream file.  Writes out the version and class structure.  The file is fileIn-able.  UniClasses will be filed out."	| url pg stamp pol |	(pg := self valueOfProperty: #SqueakPage) ifNil: [pg := SqueakPage new]		ifNotNil: 			[pg contentsMorph ~~ self 				ifTrue: 					[self inform: 'morph''s SqueakPage property is out of date'.					pg := SqueakPage new]].	(url := pg url) ifNil: 			[url := ServerDirectory defaultStemUrl , '1.sp'.	"A new legal place"			url := UIManager default 						request: 'url of a place to store this object.Must begin with file:// or ftp://'						initialAnswer: url.			url isEmpty ifTrue: [^#cancel]].	stamp := Utilities authorInitialsPerSe.	stamp isEmptyOrNil ifTrue: [ stamp := '*' ].	pg saveMorph: self author: stamp.	SqueakPageCache atURL: url put: pg.	"setProperty: #SqueakPage"	(pol := pg policy) ifNil: [pol := #neverWrite].	pg		policy: #now;		dirty: true.	pg write.	"force the write"	pg policy: pol.	^pg! !!MenuMorph methodsFor: 'construction' stamp: 'cmm 3/8/2010 22:29'!addStayUpIcons	| title closeBox pinBox titleBarArea titleString |	title := submorphs				detect: [:ea | ea hasProperty: #titleString]				ifNone: [self setProperty: #needsTitlebarWidgets toValue: true.					^ self].	closeBox := IconicButton new target: self;				 actionSelector: #delete;				 labelGraphic: self class closeBoxImage;				 color: Color transparent;				 extent: 14 @ 16;				 borderWidth: 0.	pinBox := IconicButton new target: self;				 actionSelector: #stayUp:;				 arguments: {true};				 labelGraphic: self class pushPinImage;				 color: Color transparent;				 extent: 14 @ 15;				 borderWidth: 0.	Preferences noviceMode		ifTrue: [closeBox setBalloonText: 'close this menu'.			pinBox setBalloonText: 'keep this menu up'].	titleBarArea :=  AlignmentMorph newRow vResizing: #shrinkWrap;			 layoutInset: 3;			 color: Preferences menuTitleColor;			 addMorphBack: closeBox;			 addMorphBack: title;			 addMorphBack: pinBox.		title color: Color transparent.	titleString := title 		findDeepSubmorphThat: [:each | each respondsTo: #font: ]		ifAbsent: [StringMorph contents: String empty].	titleString font: Preferences windowTitleFont.	Preferences roundedMenuCorners		ifTrue: [titleBarArea useRoundedCorners].		self addMorphFront: titleBarArea.	titleBarArea setProperty: #titleString toValue: (title valueOfProperty: #titleString).	title removeProperty: #titleString.	self setProperty: #hasTitlebarWidgets toValue: true.	self removeProperty: #needsTitlebarWidgets.	self removeStayUpItems! !