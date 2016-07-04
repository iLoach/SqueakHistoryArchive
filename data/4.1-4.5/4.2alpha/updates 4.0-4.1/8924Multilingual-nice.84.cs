"Change Set:		8924Multilingual-nice.84Multilingual-nice.84:Use literal ByteArrayMultilingual-nice.81:Correct a bug in StrikeFont (ascii assigned with a Character rather than its code).change pattern dest:=dest+(x@0) into dest:=dest x+x@dest yMultilingual-ul.82:- code criticsMultilingual-nice.83:Use selectorsAndMethodsDo: "!!TTCFontSet methodsFor: 'accessing' stamp: 'yo 11/16/2002 01:17'!pointSize	^ fontArray first pixelSize * 72 // 96.! !!TTCFontSet methodsFor: 'testing' stamp: 'yo 12/29/2003 15:02'!isTTCFont	^true! !!TTCFontSet methodsFor: 'accessing' stamp: 'yo 11/16/2002 01:16'!familySizeFace	^ Array		with: fontArray first name		with: self height		with: 0.! !!MultiCharacterBlockScanner methodsFor: 'scanning' stamp: 'nice 12/26/2009 23:28'!scanMultiCharactersCombiningFrom: startIndex to: stopIndex in: sourceString rightX: rightX stopConditions: stops kern: kernDelta	| encoding f nextDestX maxAscii startEncoding char charValue floatDestX widthAndKernedWidth nextChar |	lastIndex := startIndex.	lastIndex > stopIndex ifTrue: [lastIndex := stopIndex. ^ stops at: EndOfRun].	startEncoding := (sourceString at: startIndex) leadingChar.	font ifNil: [font := (TextConstants at: #DefaultMultiStyle) fontArray at: 1].	((font isMemberOf: StrikeFontSet) or: [font isKindOf: TTCFontSet]) ifTrue: [		f := [font fontArray at: startEncoding + 1]			on: Exception do: [:ex | nil].		f ifNil: [ f := font fontArray at: 1].		maxAscii := f maxAscii.		spaceWidth := f widthOf: Space.	] ifFalse: [		maxAscii := font maxAscii.	].	floatDestX := destX.	widthAndKernedWidth := Array new: 2.	[lastIndex <= stopIndex] whileTrue: [		encoding := (sourceString at: lastIndex) leadingChar.		encoding ~= startEncoding ifTrue: [lastIndex := lastIndex - 1. ^ stops at: EndOfRun].		char := (sourceString at: lastIndex).		charValue := char charCode.		charValue > maxAscii ifTrue: [charValue := maxAscii].		(encoding = 0 and: [(stops at: charValue + 1) ~~ nil]) ifTrue: [			^ stops at: charValue + 1		].		nextChar := (lastIndex + 1 <= stopIndex) 			ifTrue:[sourceString at: lastIndex + 1]			ifFalse:[nil].		font 			widthAndKernedWidthOfLeft: ((char isMemberOf: CombinedChar) ifTrue:[char base] ifFalse:[char]) 			right: nextChar			into: widthAndKernedWidth.		nextDestX := floatDestX + (widthAndKernedWidth at: 1).			nextDestX > rightX ifTrue: [^ stops at: CrossedX].		floatDestX := floatDestX + kernDelta + (widthAndKernedWidth at: 2).		destX := floatDestX.		lastIndex := lastIndex + 1.	].	lastIndex := stopIndex.	^ stops at: EndOfRun! !!StrikeFontSet methodsFor: 'displaying' stamp: 'nice 1/10/2010 15:07'!displayMultiString: aString on: aBitBlt from: startIndex to: stopIndex at: aPoint kern: kernDelta baselineY: baselineY	| destPoint leftX rightX glyphInfo g destY |	destPoint := aPoint.	glyphInfo := Array new: 5.	startIndex to: stopIndex do: [:charIndex |		self glyphInfoOf: (aString at: charIndex) into: glyphInfo.		g := glyphInfo at:1.		leftX := glyphInfo at:2.		rightX := glyphInfo at:3.		((glyphInfo at:5) ~= aBitBlt lastFont) ifTrue: [			(glyphInfo at:5) installOn: aBitBlt.		].		aBitBlt sourceForm: g.		destY := baselineY - (glyphInfo at:4).		aBitBlt destX: (destPoint x) destY: destY width: (rightX - leftX) height: (self height).		aBitBlt sourceOrigin: leftX @ 0.		aBitBlt copyBits.		destPoint := destPoint x + (rightX - leftX + kernDelta) @ destPoint y.	].	^ destPoint.! !!TTCFontDescription methodsFor: 'copying' stamp: 'yo 11/14/2002 22:40'!deepCopy	^ self.! !!TTCFontSet methodsFor: 'accessing' stamp: 'yo 12/10/2002 18:20'!familyName	^ 'Multi', (fontArray at: 1) familyName.! !!TTCFontSet methodsFor: 'accessing' stamp: 'yo 11/16/2002 01:16'!descentKern	^ 0.! !!StrikeFontSet methodsFor: 'displaying' stamp: 'nice 1/10/2010 15:06'!characters: anInterval in: sourceString displayAt: aPoint clippedBy: clippingRectangle rule: ruleInteger fillColor: aForm kernDelta: kernDelta on: aBitBlt	"Simple, slow, primitive method for displaying a line of characters.	No wrap-around is provided."	^anInterval inject: aPoint into: 		[:destPoint :i |		| f xTable leftX noFont sourceRect encoding ascii rightX |		encoding := (sourceString at: i) leadingChar + 1.		noFont := false.		f := [fontArray at: encoding]			on: Exception do: [:ex | nil].		f ifNil: [noFont := true. f := fontArray at: 1].		ascii := (noFont ifTrue: [$?] ifFalse: [sourceString at: i]) charCode.		(ascii < f minAscii			or: [ascii > f maxAscii])			ifTrue: [ascii := f maxAscii].		xTable := f xTable.		leftX := xTable at: ascii + 1.		rightX := xTable at: ascii + 2.		sourceRect := leftX @ 0 extent: (rightX - leftX) @ self height.		aBitBlt copyFrom: sourceRect in: f glyphs to: destPoint.		destPoint x + (rightX - leftX + kernDelta) @ destPoint y.	].! !!TTCFontSet methodsFor: 'accessing' stamp: 'yo 12/10/2002 18:28'!depth	^ (fontArray at: 1) depth.! !!MultiByteBinaryOrTextStream methodsFor: 'private' stamp: 'nice 1/18/2010 13:41'!guessConverter	^ (self originalContents includesSubString: #[27 36] asString)		ifTrue: [CompoundTextConverter new]		ifFalse: [self class defaultConverter ]! !!TTCFontSet methodsFor: 'accessing' stamp: 'TN 3/14/2005 23:46'!emphasis	^ fontArray first emphasis! !!JISX0208 class methodsFor: 'class methods' stamp: 'nice 1/10/2010 15:42'!charAtKuten: anInteger	| a b |	a := anInteger \\ 100.	b := anInteger // 100.	(a > 94 or: [b > 94]) ifTrue: [		self error: 'character code is not valid'.	].	^ Character leadingChar: self leadingChar code: ((b - 1) * 94) + a - 1.! !!SparseXTable methodsFor: 'accessing' stamp: 'nice 1/10/2010 14:54'!tableFor: code	| div |	div := code // 65536.	^xTables at: div ifAbsent: [		| table |		table := Array new: 65536 withAll: 0.		xTables at: div put: table.		table].! !!TTCFontDescription methodsFor: 'objects from disk' stamp: 'yo 11/30/2002 22:50'!objectForDataStream: refStrm	| dp |	"I am about to be written on an object file.  Write a reference to a known Font in the other system instead.  "	"A path to me"	(TextConstants at: #forceFontWriting ifAbsent: [false]) ifTrue: [^ self].		"special case for saving the default fonts on the disk.  See collectionFromFileNamed:"	dp := DiskProxy global: #TTCFontDescription selector: #descriptionNamed:at:			args: {self name. ((TTCFontDescription descriptionNamed: self name) indexOf: self)}.	refStrm replace: self with: dp.	^ dp.! !!TTCFontSet methodsFor: 'accessing' stamp: 'yo 11/16/2002 01:27'!pointSizes	^ self class pointSizes.! !!TTCFontSet methodsFor: 'accessing' stamp: 'yo 8/5/2003 15:31'!textStyle	^ TextStyle actualTextStyles		detect: [:aStyle | (aStyle fontArray collect: [:s | s name]) includes: self name]		ifNone: [].! !!TTCFontSet methodsFor: 'measuring' stamp: 'yo 11/16/2002 01:17'!widthOf: aCharacter	| encoding |	encoding := aCharacter leadingChar.	^ (fontArray at: encoding + 1) widthOf: aCharacter.! !!TTCFontSet methodsFor: 'private' stamp: 'yo 1/7/2005 11:17'!glyphInfoOf: aCharacter into: glyphInfoArray	| index f code |	index := aCharacter leadingChar + 1.	fontArray size < index ifTrue: [^ self questionGlyphInfoInto: glyphInfoArray].	(f := fontArray at: index) ifNil: [^ self questionGlyphInfoInto: glyphInfoArray].	code := aCharacter charCode.	((code between: f minAscii and: f maxAscii) not) ifTrue: [		^ self questionGlyphInfoInto: glyphInfoArray.	].	f glyphInfoOf: aCharacter into: glyphInfoArray.	glyphInfoArray at: 5 put: self.	^ glyphInfoArray.! !!TTCFontDescription methodsFor: 'accessing' stamp: 'yo 12/27/2002 04:25'!at: aCharOrInteger	| char |	char := aCharOrInteger asCharacter.	^ glyphs at: (char charCode) + 1.! !!EFontBDFFontReaderForRanges methodsFor: 'as yet unclassified' stamp: 'ul 1/11/2010 07:25'!override: chars with: otherFileName ranges: pairArray transcodingTable: table additionalRange: additionalRange	| other rangeStream newChars currentRange |	other := BDFFontReader openFileNamed: otherFileName.	rangeStream := ReadStream on: pairArray.	currentRange := rangeStream next.	newChars := PluggableSet new.	newChars hashBlock: [:elem | (elem at: 2) hash].	newChars equalBlock: [:a :b | (a at: 2) = (b at: 2)].	other readChars do: [:array | | code u j form | 		code := array at: 2.		"code printStringHex printString displayAt: 0@0."		code > currentRange last ifTrue: [			[rangeStream atEnd not and: [currentRange := rangeStream next. currentRange last < code]] whileTrue.			rangeStream atEnd ifTrue: [				newChars addAll: chars.				^ newChars.			].		].		(code between: currentRange first and: currentRange last) ifTrue: [			form := array at: 1.			form ifNotNil: [				j := array at: 2.				u := table at: (((j // 256) - 33 * 94 + ((j \\ 256) - 33)) + 1).				u ~= -1 ifTrue: [					array at: 2 put: u.					newChars add: array.					additionalRange do: [:e | | newArray |						e first = (array at: 2) ifTrue: [							newArray := array clone.							newArray at: 2 put: e second.							newChars add: newArray						].					]				].			].		].	].	self error: 'should not reach here'.! !!TTCFontSet methodsFor: 'testing' stamp: 'yo 2/12/2007 19:34'!isFontSet	^ true.! !!TTCFontSet methodsFor: 'accessing' stamp: 'nk 8/31/2004 09:27'!height	^fontArray first pixelSize.! !!MultiByteBinaryOrTextStream methodsFor: 'fileIn/Out' stamp: 'nice 1/18/2010 13:42'!setConverterForCode	| current |	current := converter saveStateOf: self.	self position: 0.	self binary.	((self next: 3) = #[ 16rEF 16rBB 16rBF ]) ifTrue: [		self converter: UTF8TextConverter new	] ifFalse: [		self converter: MacRomanTextConverter new.	].	converter restoreStateOf: self with: current.	self text.! !!TTCFontSet methodsFor: 'accessing' stamp: 'nk 8/31/2004 09:27'!lineGrid	^ fontArray first lineGrid.! !!TTCFontSet methodsFor: 'displaying' stamp: 'nice 1/10/2010 15:17'!displayString: aString on: aBitBlt from: startIndex to: stopIndex at: aPoint kern: kernDelta baselineY: baselineY	| destPoint font form encoding glyphInfo char charCode destY |	destPoint := aPoint.	glyphInfo := Array new: 5.	startIndex to: stopIndex do: [:charIndex |		char := aString at: charIndex.		encoding := char leadingChar + 1.		charCode := char charCode.		font := fontArray at: encoding.		((charCode between: font minAscii and: font maxAscii) not) ifTrue: [			charCode := font maxAscii].		self glyphInfoOf: char into: glyphInfo.		form := glyphInfo first.		(glyphInfo fifth ~= aBitBlt lastFont) ifTrue: [			glyphInfo fifth installOn: aBitBlt.		].		destY := baselineY - glyphInfo fourth. 		aBitBlt			sourceForm: form;			destX: destPoint x;			destY: destY;			sourceOrigin: 0 @ 0;			width: form width;			height: form height;			copyBits.		destPoint := destPoint x + (form width + kernDelta) @ destPoint y.	].	^ destPoint.! !!TTCFontSet methodsFor: 'private' stamp: 'yo 1/6/2005 17:16'!questionGlyphInfoInto: glyphInfoArray	| f form |	f := fontArray at: 1.	form := f formOf: $?.	glyphInfoArray at: 1 put: form;		at: 2 put: 0;		at: 3 put: form width;		at: 4 put: self.	^ glyphInfoArray.! !!TTCFontDescription methodsFor: 'accessing' stamp: 'yo 11/14/2002 18:42'!name	^ self familyName copyWithout: Character space.! !!TTCFontSet methodsFor: 'accessing' stamp: 'yo 11/16/2002 01:15'!ascent	^ (fontArray at: 1) ascent.! !!TTCFontDescription methodsFor: 'accessing' stamp: 'yo 11/14/2002 18:28'!size	^ glyphs size.! !!TTCFontSet methodsFor: 'accessing' stamp: 'yo 11/16/2002 01:16'!descent	^ (fontArray at: 1) descent.! !!TTCFontSet methodsFor: 'accessing' stamp: 'yo 11/16/2002 01:16'!baseKern	^ 0.! !!TTCFontSet methodsFor: 'accessing' stamp: 'yo 11/16/2002 01:16'!fontArray	^ fontArray! !!TTCFontSet methodsFor: 'accessing' stamp: 'yo 11/16/2002 01:16'!emphasized: code! !!StrikeFontSet methodsFor: 'copying' stamp: 'yo 9/17/2002 17:15'!copy	| s a |	s := self class new.	s name: self name.	s emphasis: self emphasis.	s reset.	a := Array new: fontArray size.	1 to: a size do: [:i |		a at: i put: (fontArray at: i) copy.	].	s fontArray: a.	^ s.! !!TTCFontSet methodsFor: 'accessing' stamp: 'yo 11/8/2004 21:18'!descentOf: aChar	^ fontArray first descentOf: aChar! !!TTCFontSet methodsFor: 'accessing' stamp: 'yo 11/8/2004 21:18'!ascentOf: aCharacter	^ fontArray first ascentOf: aCharacter.! !!StrikeFontSet methodsFor: 'displaying' stamp: 'nice 1/10/2010 15:09'!displayString: aString on: aBitBlt from: startIndex to: stopIndex at: aPoint kern: kernDelta from: fromFont baselineY: baselineY	| destPoint leftX rightX glyphInfo g tag char destY |	destPoint := aPoint.	rIndex := startIndex.	tag := (aString at: rIndex) leadingChar.	glyphInfo := Array new: 5.	[rIndex <= stopIndex] whileTrue: [		char := aString at: rIndex.		((fromFont hasGlyphOf: char) or: [char leadingChar ~= tag]) ifTrue: [^destPoint].		self glyphInfoOf: char into: glyphInfo.		g := glyphInfo first.		leftX := glyphInfo second.		rightX := glyphInfo third.		(glyphInfo fifth ~= aBitBlt lastFont) ifTrue: [			glyphInfo fifth installOn: aBitBlt.		].		destY := baselineY - glyphInfo fourth. 		aBitBlt			sourceForm: g;			destX: destPoint x;			destY: destY;			sourceOrigin: leftX @ 0;			width: rightX - leftX;			height: self height;			copyBits.		destPoint := destPoint x + (rightX - leftX + kernDelta) @ destPoint y.		rIndex := rIndex + 1.	].	^destPoint.! !!TTCFontSet methodsFor: 'displaying' stamp: 'yo 1/7/2005 12:05'!displayString: aString on: aBitBlt from: startIndex to: stopIndex at: aPoint kern: kernDelta	^ self displayString: aString on: aBitBlt from: startIndex to: stopIndex at: aPoint kern: kernDelta baselineY: aPoint y + self ascent.! !!MultiByteFileStream methodsFor: 'private' stamp: 'nice 1/18/2010 13:42'!setConverterForCode	| current |	(SourceFiles at: 2)		ifNotNil: [self fullName = (SourceFiles at: 2) fullName ifTrue: [^ self]].	current := self converter saveStateOf: self.	self position: 0.	self binary.	((self next: 3) = #[ 16rEF 16rBB 16rBF ]) ifTrue: [		self converter: UTF8TextConverter new	] ifFalse: [		self converter: MacRomanTextConverter new.	].	converter restoreStateOf: self with: current.	self text.! !!JapaneseEnvironment class methodsFor: 'subclass responsibilities' stamp: 'ul 1/11/2010 07:09'!systemConverterClass	| platformName osVersion encoding |	platformName := SmalltalkImage current platformName.	osVersion := SmalltalkImage current osVersion.	(platformName = 'Win32' and: [osVersion = 'CE']) 		ifTrue: [^UTF8TextConverter].	(#('Win32' 'ZaurusOS') includes: platformName) 		ifTrue: [^ShiftJISTextConverter].	platformName = 'Mac OS' 		ifTrue: 			[^('10*' match: osVersion) 				ifTrue: [UTF8TextConverter]				ifFalse: [ShiftJISTextConverter]].	platformName = 'unix' 		ifTrue: 			[encoding := X11Encoding encoding.			encoding ifNil: [^EUCJPTextConverter].			(encoding = 'utf-8') 				ifTrue: [^UTF8TextConverter].							(encoding = 'shiftjis' or: [ encoding = 'sjis' ]) 				ifTrue: [^ShiftJISTextConverter].							^EUCJPTextConverter].	^MacRomanTextConverter! !!LanguageEditor class methodsFor: 'initialize-release' stamp: 'ul 1/11/2010 07:17'!initCheckMethods	"LanguageEditor initCheckMethods"	| registry |	registry := Dictionary new		at: 'es' put: #checkSpanishPhrase:translation:;		yourself.	^registry! !!StrikeFontSet methodsFor: 'displaying' stamp: 'nice 1/10/2010 15:08'!displayStringR2L: aString on: aBitBlt from: startIndex to: stopIndex at: aPoint kern: kernDelta 	| destPoint font |	destPoint := aPoint.	startIndex to: stopIndex do: [:charIndex | 		| encoding ascii xTable leftX rightX | 		encoding := (aString at: charIndex) leadingChar + 1.		ascii := (aString at: charIndex) charCode.		font := fontArray at: encoding.		((ascii between: font minAscii and: font maxAscii) not) ifTrue: [			ascii := font maxAscii].		xTable := font xTable.		leftX := xTable at: ascii + 1.		rightX := xTable at: ascii + 2.		aBitBlt			sourceForm: font glyphs;			destX: destPoint x - (rightX - leftX);			destY: destPoint y;			sourceOrigin: leftX @ 0;			width: rightX - leftX;			height: self height;			copyBits.		destPoint := destPoint x - (rightX - leftX + kernDelta) @ destPoint y.	].! !!TTCFontDescription methodsFor: 'copying' stamp: 'yo 11/14/2002 22:41'!veryDeepCopyWith: deepCopier	"Return self.  I am shared.  Do not record me."! !!TTCFontSet methodsFor: 'displaying' stamp: 'nice 1/10/2010 15:18'!displayStringR2L: aString on: aBitBlt from: startIndex to: stopIndex at: aPoint kern: kernDelta 	| destPoint font form encoding char charCode glyphInfo |	destPoint := aPoint.	glyphInfo := Array new: 5.	startIndex to: stopIndex do: [:charIndex |		char := aString at: charIndex.		encoding := char leadingChar + 1.		charCode := char charCode.		font := fontArray at: encoding.		((charCode between: font minAscii and: font maxAscii) not) ifTrue: [			charCode := font maxAscii].		self glyphInfoOf: char into: glyphInfo.		form := glyphInfo first.			(glyphInfo size > 4 and: [glyphInfo fifth notNil and: [glyphInfo fifth ~= aBitBlt lastFont]]) ifTrue: [				glyphInfo fifth installOn: aBitBlt.			].		aBitBlt			sourceForm: form;			destX: destPoint x - form width;			destY: destPoint y;			sourceOrigin: 0 @ 0;			width: form width;			height: form height;			copyBits.		destPoint := destPoint x - (form width + kernDelta) @ destPoint y.	].! !!TTCFont methodsFor: 'emphasis' stamp: 'dgd 5/19/2007 16:19'!setupDefaultFallbackFont	| fonts f |	fonts := TextStyle default fontArray.	f := fonts first.	1 to: fonts size do: [:i |		self height > (fonts at: i) height ifTrue: [f := fonts at: i].	].	(f == self)		ifFalse:[ self fallbackFont: f ].	self reset.! !!SystemNavigation methodsFor: '*Multilingual-Editor' stamp: 'nice 1/15/2010 23:01'!allSelect: aBlock 	"Answer a SortedCollection of each method that, when used as 	the block  	argument to aBlock, gives a true result."	| aCollection |	aCollection := SortedCollection new.	Cursor execute		showWhile: [self				allBehaviorsDo: [:class | class						selectorsAndMethodsDo: [:sel :m | (aBlock value: m)								ifTrue: [aCollection add: class name , ' ' , sel]]]].	^ aCollection! !!TTCFontSet methodsFor: 'accessing' stamp: 'yo 11/16/2002 01:17'!maxAsciiFor: encoding	| f |	f := (fontArray at: encoding+1).	f ifNotNil: [^ f maxAscii].	^ 0.! !