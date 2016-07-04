"Change Set:		7919Multilingual-nice.51Multilingual-nice.51:Fix http://bugs.squeak.org/view.php?id=6450Let MultiCharacterScanner honour the stops argument rather than using stopConditions instVar."!!MultiCharacterBlockScanner methodsFor: 'stop conditions' stamp: 'nice 10/5/2009 22:11'!scanMultiCharactersCombiningFrom: startIndex to: stopIndex in: sourceString rightX: rightX stopConditions: stops kern: kernDelta	| encoding f nextDestX maxAscii startEncoding char charValue floatDestX widthAndKernedWidth nextChar |	lastIndex := startIndex.	lastIndex > stopIndex ifTrue: [lastIndex := stopIndex. ^ stops at: EndOfRun].	startEncoding := (sourceString at: startIndex) leadingChar.	font ifNil: [font := (TextConstants at: #DefaultMultiStyle) fontArray at: 1].	((font isMemberOf: StrikeFontSet) or: [font isKindOf: TTCFontSet]) ifTrue: [		[f := font fontArray at: startEncoding + 1]			on: Exception do: [:ex | f := font fontArray at: 1].		f ifNil: [ f := font fontArray at: 1].		maxAscii := f maxAscii.		spaceWidth := f widthOf: Space.	] ifFalse: [		maxAscii := font maxAscii.	].	floatDestX := destX.	widthAndKernedWidth := Array new: 2.	[lastIndex <= stopIndex] whileTrue: [		encoding := (sourceString at: lastIndex) leadingChar.		encoding ~= startEncoding ifTrue: [lastIndex := lastIndex - 1. ^ stops at: EndOfRun].		char := (sourceString at: lastIndex).		charValue := char charCode.		charValue > maxAscii ifTrue: [charValue := maxAscii].		(encoding = 0 and: [(stops at: charValue + 1) ~~ nil]) ifTrue: [			^ stops at: charValue + 1		].		nextChar := (lastIndex + 1 <= stopIndex) 			ifTrue:[sourceString at: lastIndex + 1]			ifFalse:[nil].		font 			widthAndKernedWidthOfLeft: ((char isMemberOf: CombinedChar) ifTrue:[char base] ifFalse:[char]) 			right: nextChar			into: widthAndKernedWidth.		nextDestX := floatDestX + (widthAndKernedWidth at: 1).			nextDestX > rightX ifTrue: [^ stops at: CrossedX].		floatDestX := floatDestX + kernDelta + (widthAndKernedWidth at: 2).		destX := floatDestX.		lastIndex := lastIndex + 1.	].	lastIndex := stopIndex.	^ stops at: EndOfRun! !!MultiCharacterScanner methodsFor: 'scanning' stamp: 'nice 10/5/2009 22:12'!scanMultiCharactersFrom: startIndex to: stopIndex in: sourceString rightX: rightX stopConditions: stops kern: kernDelta	| ascii encoding f nextDestX maxAscii startEncoding floatDestX widthAndKernedWidth nextChar atEndOfRun |	lastIndex := startIndex.	lastIndex > stopIndex ifTrue: [lastIndex := stopIndex. ^ stops at: EndOfRun].	startEncoding := (sourceString at: startIndex) leadingChar.	font ifNil: [font := (TextConstants at: #DefaultMultiStyle) fontArray at: 1].	((font isMemberOf: StrikeFontSet) or: [font isKindOf: TTCFontSet]) ifTrue: [		[f := font fontArray at: startEncoding + 1]			on: Exception do: [:ex | f := font fontArray at: 1].		f ifNil: [ f := font fontArray at: 1].		maxAscii := f maxAscii.		spaceWidth := f widthOf: Space.	] ifFalse: [		maxAscii := font maxAscii.	].	floatDestX := destX.	widthAndKernedWidth := Array new: 2.	atEndOfRun := false.	[lastIndex <= stopIndex] whileTrue: [		encoding := (sourceString at: lastIndex) leadingChar.		encoding ~= startEncoding ifTrue: [lastIndex := lastIndex - 1. ^ stops at: EndOfRun].		ascii := (sourceString at: lastIndex) charCode.		ascii > maxAscii ifTrue: [ascii := maxAscii].		(encoding = 0 and: [ascii < stops size and: [(stops at: ascii + 1) ~~ nil]]) ifTrue: [^ stops at: ascii + 1].		(self isBreakableAt: lastIndex in: sourceString in: Latin1Environment) ifTrue: [			self registerBreakableIndex.		].		nextChar := (lastIndex + 1 <= stopIndex) 			ifTrue:[sourceString at: lastIndex + 1]			ifFalse:[				atEndOfRun := true.				"if there is a next char in sourceString, then get the kern 				and store it in pendingKernX"				lastIndex + 1 <= sourceString size					ifTrue:[sourceString at: lastIndex + 1]					ifFalse:[	nil]].		font 			widthAndKernedWidthOfLeft: (sourceString at: lastIndex) 			right: nextChar			into: widthAndKernedWidth.		nextDestX := floatDestX + (widthAndKernedWidth at: 1).		nextDestX > rightX ifTrue: [destX ~= firstDestX ifTrue: [^stops at: CrossedX]].		floatDestX := floatDestX + kernDelta + (widthAndKernedWidth at: 2).		atEndOfRun 			ifTrue:[				pendingKernX := (widthAndKernedWidth at: 2) - (widthAndKernedWidth at: 1).				floatDestX := floatDestX - pendingKernX].		destX := floatDestX .		lastIndex := lastIndex + 1.	].	lastIndex := stopIndex.	^ stops at: EndOfRun! !