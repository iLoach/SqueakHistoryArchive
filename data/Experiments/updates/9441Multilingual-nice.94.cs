'From Squeak3.11alpha of 13 February 2010 [latest update: #9483] on 9 March 2010 at 11:11:23 am'!!EncodedCharSet class methodsFor: 'class methods' stamp: 'nice 2/28/2010 16:38'!initialize"	self initialize"	self allSubclassesDo: [:each | each initialize].	EncodedCharSets := Array new: 256.	EncodedCharSets at: 0+1 put: Unicode "Latin1Environment".	EncodedCharSets at: 1+1 put: JISX0208.	EncodedCharSets at: 2+1 put: GB2312.	EncodedCharSets at: 3+1 put: KSX1001.	EncodedCharSets at: 4+1 put: JISX0208.	EncodedCharSets at: 5+1 put: JapaneseEnvironment.	EncodedCharSets at: 6+1 put: SimplifiedChineseEnvironment.	EncodedCharSets at: 7+1 put: KoreanEnvironment.	EncodedCharSets at: 8+1 put: GB2312.	"EncodedCharSets at: 9+1 put: UnicodeTraditionalChinese."	"EncodedCharSets at: 10+1 put: UnicodeVietnamese."	EncodedCharSets at: 12+1 put: KSX1001.	EncodedCharSets at: 13+1 put: GreekEnvironment.	EncodedCharSets at: 14+1 put: Latin2Environment.	EncodedCharSets at: 15+1 put: RussianEnvironment.	EncodedCharSets at: 256 put: Unicode.! !!MultiCharacterScanner methodsFor: 'initialize' stamp: 'ul 3/8/2010 04:55'!initializeStringMeasurer	stopConditions := TextStopConditions new! !!MultiCharacterScanner class methodsFor: 'class initialization' stamp: 'nice 3/8/2010 11:55'!initialize"	MultiCharacterScanner initialize"	| a |	a := TextStopConditions new.	a at: 1 + 1 put: #embeddedObject.	a at: Tab asciiValue + 1 put: #tab.	a at: CR asciiValue + 1 put: #cr.	a at: Character lf asciiValue + 1 put: #cr.		NilCondition := a copy.	DefaultStopConditions := a copy.	PaddedSpaceCondition := a copy.	PaddedSpaceCondition at: Space asciiValue + 1 put: #paddedSpace.		SpaceCondition := a copy.	SpaceCondition at: Space asciiValue + 1 put: #space.! !!Unicode class methodsFor: 'class methods' stamp: 'nice 2/28/2010 16:34'!leadingChar	^ 0! !MultiCharacterScanner initialize!EncodedCharSet initialize!