"Change Set:		8450MorphicExtras-ul.61MorphicExtras-ul.61:- replace sends of #ifNotNilDo: to #ifNotNil:, #ifNil:ifNotNilDo: to #ifNil:ifNotNil:, #ifNotNilDo:ifNil: to #ifNotNil:ifNil:"!!BookMorph methodsFor: 'other' stamp: 'ul 12/12/2009 14:12'!adjustCurrentPageForFullScreen	"Adjust current page to conform to whether or not I am in full-screen mode.  Also, enforce uniform page size constraint if appropriate"	self isInFullScreenMode		ifTrue:			[(currentPage hasProperty: #sizeWhenNotFullScreen) ifFalse:				[currentPage setProperty: #sizeWhenNotFullScreen toValue: currentPage extent].			currentPage extent: Display extent]		ifFalse:			[(currentPage hasProperty: #sizeWhenNotFullScreen) ifTrue:				[currentPage extent: (currentPage valueOfProperty: #sizeWhenNotFullScreen).				currentPage removeProperty: #sizeWhenNotFullScreen].			self uniformPageSize ifNotNil:				[:anExtent | currentPage extent: anExtent]].	(self valueOfProperty: #floatingPageControls) ifNotNil:		[:pc | pc isInWorld ifFalse: [pc openInWorld]]! !!URLMorph methodsFor: 'private' stamp: 'ul 12/12/2009 14:04'!enclosingPage	"Answer the inner-most SqueakPage contents that contains this morph, or nil if there isn't one."	self allOwnersDo:		[:m | (m isKindOf: PasteUpMorph)			ifTrue: [(SqueakPageCache pageForMorph: m) ifNotNil: [:pg | ^ pg]]].	^ nil! !!Flaps class methodsFor: 'predefined flaps' stamp: 'ul 12/12/2009 14:11'!twiddleSuppliesButtonsIn: aStrip	"Munge item(s) in the strip whose names as seen in the parts bin should be different from the names to be given to resulting torn-off instances"	(aStrip submorphs detect: [:m | m target == StickyPadMorph] ifNone: [nil])		ifNotNil:			[:aButton | aButton arguments: {#newStandAlone.  'tear off'}]! !!PostscriptCanvas methodsFor: 'private' stamp: 'ul 12/12/2009 14:07'!textStyled: s at: ignored0 font: ignored1 color: c justified: justify parwidth: parwidth 	| fillC oldC |	fillC := c.	self shadowColor		ifNotNil: [:sc | 			self comment: ' shadow color: ' , sc printString.			fillC := sc].	self comment: ' text color: ' , c printString.	oldC := currentColor.	self setColor: fillC.	self comment: '  origin ' , origin printString.	"self moveto: point."	"now done by sender"	target print: ' (';		 print: s asPostscript;		 print: ') '.	justify		ifTrue: [target write: parwidth;				 print: ' jshow';				 cr]		ifFalse: [target print: 'show'].	target cr.	self setColor: oldC.! !!Flaps class methodsFor: 'shared flaps' stamp: 'ul 12/12/2009 14:12'!sharedFlapsAlongBottom	"Put all shared flaps (except Painting which can't be moved) along the bottom"	"Flaps sharedFlapsAlongBottom"	| leftX unordered ordered |	unordered _ self globalFlapTabsIfAny asIdentitySet.	ordered _ Array streamContents:		[:s | {				'Squeak' translated.				'Navigator' translated.				'Supplies' translated.				'Widgets' translated.				'Stack Tools' translated.				'Tools' translated.				'Painting' translated.			} do:			[:id | (self globalFlapTabWithID: id) ifNotNil:				[:ft | unordered remove: ft.				id = 'Painting' translated ifFalse: [s nextPut: ft]]]].	"Pace off in order from right to left, setting positions"	leftX _ Display width-15.	ordered , unordered asArray reverseDo:		[:ft | ft setEdge: #bottom.		ft right: leftX - 3.  leftX _ ft left].	"Put Nav Bar centered under tab if possible"	(self globalFlapTabWithID: 'Navigator' translated) ifNotNil:		[:ft | ft referent left: (ft center x - (ft referent width//2) max: 0)].	self positionNavigatorAndOtherFlapsAccordingToPreference.! !