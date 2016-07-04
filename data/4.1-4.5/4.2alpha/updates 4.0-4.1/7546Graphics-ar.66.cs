"Change Set:		7546Graphics-ar.66Graphics-ar.66:Several font display fixes:- BitBlt primDisplayString needs to consult the glyph map in its non-primitive code path- CharacterScanner will no longer interpret characters 256/257 as stop conditions when occuring in a string- The 'stops' argument is used consistently instead of the ivar stopConditions- StrikeFont m17n display should not pass on the entire remaining string to its fallback font- The default fallbackFont substitution font *must* use the font it is backing since the fallback font will not be installed independently from the original.Graphics-ar.65:Fixes StrikeFont>>displayMultiString: to do proper two-pass rendering for fonts requiring two-pass rendering."!!BitBlt methodsFor: 'private' stamp: 'ar 8/15/2009 11:16'!primDisplayString: aString from: startIndex to: stopIndex map: glyphMap xTable: xTable kern: kernDelta	| ascii |	<primitive:'primitiveDisplayString' module:'BitBltPlugin'>	startIndex to: stopIndex do:[:charIndex|		ascii := (aString at: charIndex) asciiValue.		glyphMap ifNotNil:[ascii := glyphMap at: ascii+1].		sourceX := xTable at: ascii + 1.		width := (xTable at: ascii + 2) - sourceX.		self copyBits.		destX := destX + width + kernDelta.	].! !!StrikeFont methodsFor: 'accessing' stamp: 'ar 8/15/2009 11:25'!fallbackFont	"Answers the fallbackFont for the receiver. The fallback font must be some derivative of the receiver since it will not be asked to install itself properly on the target BitBlt so rendering a completely different font here is simply not possible. The default implementation uses a synthetic font that maps all characters to question marks."	^ fallbackFont		ifNil: [fallbackFont := FixedFaceFont new errorFont baseFont: self]! !!CharacterScanner methodsFor: 'scanner methods' stamp: 'ar 8/15/2009 11:19'!scanMultiCharactersFrom: startIndex to: stopIndex in: sourceString rightX: rightX stopConditions: stops kern: kernDelta	| ascii encoding f nextDestX startEncoding |	lastIndex := startIndex.	lastIndex > stopIndex ifTrue: [lastIndex := stopIndex. ^ stops at: EndOfRun].	startEncoding := (sourceString at: startIndex) leadingChar.	font ifNil: [font := (TextConstants at: #DefaultMultiStyle) fontArray at: 1].	((font isMemberOf: StrikeFontSet) or: [font isKindOf: TTCFontSet]) ifTrue: [		f := font fontArray at: startEncoding + 1.		spaceWidth := f widthOf: Space.	].	[lastIndex <= stopIndex] whileTrue: [		encoding := (sourceString at: lastIndex) leadingChar.		encoding ~= startEncoding ifTrue: [lastIndex := lastIndex - 1. ^ stops at: EndOfRun].		ascii := (sourceString at: lastIndex) charCode.		(encoding = 0 and: [ascii < 256 and:[(stops at: ascii + 1) notNil]]) 			ifTrue: [^ stops at: ascii + 1].		nextDestX := destX + (font widthOf: (sourceString at: lastIndex)).		nextDestX > rightX ifTrue: [^ stops at: CrossedX].		destX := nextDestX + kernDelta.		"destX printString displayAt: 0@(lastIndex*20)."		lastIndex := lastIndex + 1.	].	lastIndex := stopIndex.	^ stops at: EndOfRun! !!StrikeFont methodsFor: 'displaying' stamp: 'ar 8/15/2009 10:55'!displayMultiString: aString on: aBitBlt from: startIndex to: stopIndex at: aPoint kern: kernDelta baselineY: baselineY	| leftX rightX glyphInfo char destY form gfont destX destPt |	destX := aPoint x.	charIndex := startIndex.	glyphInfo := Array new: 5.	startIndex to: stopIndex do:[:charIndex|		char := aString at: charIndex.		(self hasGlyphOf: char) ifTrue: [			self glyphInfoOf: char into: glyphInfo.			form := glyphInfo at: 1.			leftX := glyphInfo at: 2.			rightX := glyphInfo at: 3.			destY := glyphInfo at: 4.			gfont := glyphInfo at: 5.			(gfont == aBitBlt lastFont) ifFalse: [gfont installOn: aBitBlt].			destY := baselineY - destY. 			aBitBlt displayGlyph: form at: destX @ destY left: leftX right: rightX font: self.			destX := destX + (rightX - leftX + kernDelta).		] ifFalse:[			destPt := self fallbackFont displayString: aString on: aBitBlt from: charIndex to: charIndex at: destX @ aPoint y kern: kernDelta from: self baselineY: baselineY.			destX := destPt x.		].	].	^destX @ aPoint y! !!BitBlt methodsFor: 'copying' stamp: 'ar 8/14/2009 20:26'!displayGlyph: aForm at: aPoint left: leftX right: rightX font: aFont	"Display a glyph in a multi-lingual font. Do 2 pass rendering if necessary.	This happens when #installStrikeFont:foregroundColor:backgroundColor: sets rule 37 (rgbMul).	the desired effect is to do two bitblt calls. The first one is with rule 37 and special colormap.	The second one is rule 34, with a colormap for applying the requested foreground color.	This two together do component alpha blending, i.e. alpha blend red, green and blue separatedly.	This is needed for arbitrary color over abitrary background text with subpixel AA."	| prevRule secondPassMap |	self sourceForm: aForm.	destX := aPoint x.	destY := aPoint y.	sourceX := leftX.	sourceY := 0.	width := rightX - leftX.	height := aFont height.	combinationRule = 37 ifTrue:[		"We need to do a second pass. The colormap set is for use in the second pass."		secondPassMap := colorMap.		colorMap := sourceForm depth = destForm depth			ifFalse: [ self cachedFontColormapFrom: sourceForm depth to: destForm depth ].		self copyBits.		prevRule := combinationRule.		combinationRule := 20. "rgbAdd"		colorMap := secondPassMap.		self copyBits.		combinationRule := prevRule.	] ifFalse:[self copyBits].! !!CharacterScanner methodsFor: 'scanner methods' stamp: 'ar 8/15/2009 11:20'!scanJapaneseCharactersFrom: startIndex to: stopIndex in: sourceString rightX: rightX stopConditions: stops kern: kernDelta	| ascii encoding f nextDestX startEncoding |	lastIndex := startIndex.	lastIndex > stopIndex ifTrue: [lastIndex := stopIndex. ^ stops at: EndOfRun].	startEncoding := (sourceString at: startIndex) leadingChar.	font ifNil: [font := (TextConstants at: #DefaultMultiStyle) fontArray at: 1].	((font isMemberOf: StrikeFontSet) or: [font isKindOf: TTCFontSet]) ifTrue: [		f := font fontArray at: startEncoding + 1.		spaceWidth := f widthOf: Space.	].	[lastIndex <= stopIndex] whileTrue: [		encoding := (sourceString at: lastIndex) leadingChar.		encoding ~= startEncoding ifTrue: [lastIndex := lastIndex - 1. ^ stops at: EndOfRun].		ascii := (sourceString at: lastIndex) charCode.		(encoding = 0 and: [ascii < 256 and:[(stops at: ascii + 1) notNil]]) 			ifTrue: [^ stops at: ascii + 1].		nextDestX := destX + (font widthOf: (sourceString at: lastIndex)).		nextDestX > rightX ifTrue: [^ stops at: CrossedX].		destX := nextDestX + kernDelta.		"destX printString displayAt: 0@(lastIndex*20)."		lastIndex := lastIndex + 1.	].	lastIndex := stopIndex.	^ stops at: EndOfRun! !