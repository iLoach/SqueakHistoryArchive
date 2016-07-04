"Change Set:		9541Graphics-nice.113Graphics-nice.113:Fix endOfRun and crossedX encodings in paragraph composition - Part 2"!!CharacterScanner methodsFor: 'scanning' stamp: 'nice 2/28/2010 18:17'!basicScanCharactersFrom: startIndex to: stopIndex in: sourceString rightX: rightX stopConditions: stops kern: kernDelta	"Primitive. This is the inner loop of text display--but see 	scanCharactersFrom: to:rightX: which would get the string, 	stopConditions and displaying from the instance. March through source 	String from startIndex to stopIndex. If any character is flagged with a 	non-nil entry in stops, then return the corresponding value. Determine 	width of each character from xTable, indexed by map. 	If dextX would exceed rightX, then return stops at: 258. 	Advance destX by the width of the character. If stopIndex has been	reached, then return stops at: 257. Optional. 	See Object documentation whatIsAPrimitive."	| ascii nextDestX char  floatDestX widthAndKernedWidth nextChar atEndOfRun |	<primitive: 103>	lastIndex := startIndex.	floatDestX := destX.	widthAndKernedWidth := Array new: 2.	atEndOfRun := false.	[lastIndex <= stopIndex]		whileTrue: 			[char := (sourceString at: lastIndex).			ascii := char asciiValue + 1.			(stops at: ascii) == nil ifFalse: [^stops at: ascii].			"Note: The following is querying the font about the width			since the primitive may have failed due to a non-trivial			mapping of characters to glyphs or a non-existing xTable."			nextChar := (lastIndex + 1 <= stopIndex) 				ifTrue:[sourceString at: lastIndex + 1]				ifFalse:[					atEndOfRun := true.					"if there is a next char in sourceString, then get the kern 					and store it in pendingKernX"					lastIndex + 1 <= sourceString size						ifTrue:[sourceString at: lastIndex + 1]						ifFalse:[	nil]].			font 				widthAndKernedWidthOfLeft: char 				right: nextChar				into: widthAndKernedWidth.			nextDestX := floatDestX + (widthAndKernedWidth at: 1).			nextDestX > rightX ifTrue: [^stops crossedX].			floatDestX := floatDestX + kernDelta + (widthAndKernedWidth at: 2).			atEndOfRun 				ifTrue:[					pendingKernX := (widthAndKernedWidth at: 2) - (widthAndKernedWidth at: 1).					floatDestX := floatDestX - pendingKernX].			destX := floatDestX.			lastIndex := lastIndex + 1].	lastIndex := stopIndex.	^stops endOfRun! !!CharacterScanner methodsFor: 'scanning' stamp: 'nice 2/28/2010 18:17'!scanCharactersFrom: startIndex to: stopIndex in: sourceString rightX: rightX stopConditions: stops kern: kernDelta	| startEncoding selector |	(sourceString isByteString) ifTrue: [^ self basicScanCharactersFrom: startIndex to: stopIndex in: sourceString rightX: rightX stopConditions: stops kern: kernDelta.].	(sourceString isWideString) ifTrue: [		startIndex > stopIndex ifTrue: [lastIndex := stopIndex. ^ stops endOfRun].		startEncoding :=  (sourceString at: startIndex) leadingChar.		selector := EncodedCharSet scanSelectorAt: startEncoding.		^ self perform: selector withArguments: (Array with: startIndex with: stopIndex with: sourceString with: rightX with: stops with: kernDelta).	].		^ stops endOfRun! !!CharacterScanner methodsFor: 'scanner methods' stamp: 'nice 2/28/2010 18:21'!scanMultiCharactersFrom: startIndex to: stopIndex in: sourceString rightX: rightX stopConditions: stops kern: kernDelta	| ascii encoding f nextDestX startEncoding |	lastIndex := startIndex.	lastIndex > stopIndex ifTrue: [lastIndex := stopIndex. ^ stops endOfRun].	startEncoding := (sourceString at: startIndex) leadingChar.	font ifNil: [font := (TextConstants at: #DefaultMultiStyle) fontArray at: 1].	((font isMemberOf: StrikeFontSet) or: [font isKindOf: TTCFontSet]) ifTrue: [		f := font fontArray at: startEncoding + 1.		spaceWidth := f widthOf: Space.	].	[lastIndex <= stopIndex] whileTrue: [		encoding := (sourceString at: lastIndex) leadingChar.		encoding ~= startEncoding ifTrue: [lastIndex := lastIndex - 1. ^ stops endOfRun].		ascii := (sourceString at: lastIndex) charCode.		(encoding = 0 and: [ascii < 256 and:[(stops at: ascii + 1) notNil]]) 			ifTrue: [^ stops at: ascii + 1].		nextDestX := destX + (font widthOf: (sourceString at: lastIndex)).		nextDestX > rightX ifTrue: [^ stops crossedX].		destX := nextDestX + kernDelta.		"destX printString displayAt: 0@(lastIndex*20)."		lastIndex := lastIndex + 1.	].	lastIndex := stopIndex.	^ stops endOfRun! !!CharacterScanner methodsFor: 'scanner methods' stamp: 'nice 2/28/2010 18:20'!scanJapaneseCharactersFrom: startIndex to: stopIndex in: sourceString rightX: rightX stopConditions: stops kern: kernDelta	| ascii encoding f nextDestX startEncoding |	lastIndex := startIndex.	lastIndex > stopIndex ifTrue: [lastIndex := stopIndex. ^ stops endOfRun].	startEncoding := (sourceString at: startIndex) leadingChar.	font ifNil: [font := (TextConstants at: #DefaultMultiStyle) fontArray at: 1].	((font isMemberOf: StrikeFontSet) or: [font isKindOf: TTCFontSet]) ifTrue: [		f := font fontArray at: startEncoding + 1.		spaceWidth := f widthOf: Space.	].	[lastIndex <= stopIndex] whileTrue: [		encoding := (sourceString at: lastIndex) leadingChar.		encoding ~= startEncoding ifTrue: [lastIndex := lastIndex - 1. ^ stops endOfRun].		ascii := (sourceString at: lastIndex) charCode.		(encoding = 0 and: [ascii < 256 and:[(stops at: ascii + 1) notNil]]) 			ifTrue: [^ stops at: ascii + 1].		nextDestX := destX + (font widthOf: (sourceString at: lastIndex)).		nextDestX > rightX ifTrue: [^ stops crossedX].		destX := nextDestX + kernDelta.		"destX printString displayAt: 0@(lastIndex*20)."		lastIndex := lastIndex + 1.	].	lastIndex := stopIndex.	^ stops endOfRun! !