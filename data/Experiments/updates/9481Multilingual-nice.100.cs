'From Squeak3.11alpha of 13 February 2010 [latest update: #9483] on 9 March 2010 at 11:11:22 am'!!LanguageEnvironment class methodsFor: 'private' stamp: 'nice 3/6/2010 22:18'!win32VMUsesUnicode	| buildDate ind date vmHead |	vmHead := Smalltalk vmVersion.	vmHead ifNil: [^ false].	buildDate := Smalltalk buildDate.	buildDate ifNil: [^ false].	ind := buildDate indexOfSubCollection: 'on'.	date := Date readFromString: (buildDate copyFrom: ind+3 to: buildDate size).	(vmHead beginsWith: 'Croquet') ifTrue: [		^ date >= (Date readFromString: '1 Feb 2007')	].	(vmHead beginsWith: 'Squeak') ifTrue: [		^ date >= (Date readFromString: '5 June 2007')	].	^ false."LanguageEnvironment win32VMUsesUnicode"! !!MultiCharacterScanner methodsFor: 'initialize' stamp: 'ul 3/8/2010 04:55'!initializeStringMeasurer	stopConditions := TextStopConditions new! !!MultiCharacterScanner class methodsFor: 'class initialization' stamp: 'nice 3/8/2010 11:55'!initialize"	MultiCharacterScanner initialize"	| a |	a := TextStopConditions new.	a at: 1 + 1 put: #embeddedObject.	a at: Tab asciiValue + 1 put: #tab.	a at: CR asciiValue + 1 put: #cr.	a at: Character lf asciiValue + 1 put: #cr.		NilCondition := a copy.	DefaultStopConditions := a copy.	PaddedSpaceCondition := a copy.	PaddedSpaceCondition at: Space asciiValue + 1 put: #paddedSpace.		SpaceCondition := a copy.	SpaceCondition at: Space asciiValue + 1 put: #space.! !MultiCharacterScanner initialize!