"Change Set:		8033Nebraska-nice.20Nebraska-nice.20:Use #keys rather than #fasterKeysNote that pattern (x keys asArray sort) could as well be written (x keys sort) now that keys returns an Array...This #asArray is here solely for cross-dialect/fork compatibility."!!CanvasEncoder class methodsFor: 'as yet unclassified' stamp: 'nice 10/21/2009 00:02'!showStats"CanvasEncoder showStats"	| answer bucket |	SentTypesAndSizes ifNil: [^Beeper beep].	answer := WriteStream on: String new.	SentTypesAndSizes keys asArray sort do: [ :each |		bucket := SentTypesAndSizes at: each.		answer nextPutAll: each printString,' ',				bucket first printString,'  ',				bucket second asStringWithCommas,' ',				(self nameForCode: each); cr.	].	StringHolder new contents: answer contents; openLabel: 'send/receive stats'.! !!StringSocket class methodsFor: 'as yet unclassified' stamp: 'nice 10/21/2009 00:14'!showRatesSeen"StringSocket showRatesSeen"	| answer |	MaxRatesSeen ifNil: [^Beeper beep].	answer := WriteStream on: String new.	MaxRatesSeen keys asArray sort do: [ :key |		answer nextPutAll: key printString,'  ',((MaxRatesSeen at: key) // 10000) printString; cr	].	StringHolder new contents: answer contents; openLabel: 'send rates at 10 second intervals'.! !