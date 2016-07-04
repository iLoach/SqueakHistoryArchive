"Change Set:		7542Collections-nice.116Collections-nice.116:Patch forhttp://bugs.squeak.org/view.php?id=7269OrderedCollection streamContents brokenhttp://bugs.squeak.org/view.php?id=2390ReadWriteStream with an OrderedCollection bugs when growing  Collections-nice.115:Patch forhttp://bugs.squeak.org/view.php?id=7296forceTo:paddingWith: doesn't work with OrderedCollections "!!SequenceableCollection methodsFor: 'copying' stamp: 'nice 2/26/2009 11:48'!forceTo: length paddingStartWith: elem 	"Force the length of the collection to length, padding  	the beginning of the result if necessary with elem.  	Note that this makes a copy."	| newCollection padLen |	newCollection := self species ofSize: length.	padLen := length - self size max: 0.	newCollection		from: 1		to: padLen		put: elem.	newCollection		replaceFrom: padLen + 1		to: ((padLen + self size) min: length)		with: self		startingAt:  1.	^ newCollection! !!OrderedCollection class methodsFor: 'instance creation' stamp: 'nice 2/24/2009 11:30'!new: anInteger withAll: anObject	^ super basicNew setContents: (Array new: anInteger withAll: anObject)! !!SequenceableCollection methodsFor: 'copying' stamp: 'nice 2/24/2009 11:33'!forceTo: length paddingWith: elem	"Force the length of the collection to length, padding	if necessary with elem.  Note that this makes a copy."	| newCollection |	newCollection := self species new: length withAll: elem.	newCollection replaceFrom: 1 to: (self size min: length) with: self startingAt: 1.	^ newCollection! !!SequenceableCollection methodsFor: 'copying' stamp: 'nice 2/26/2009 11:26'!grownBy: length	"Answer a copy of receiver collection with size grown by length"	| newCollection |	newCollection := self species ofSize: self size + length.	newCollection replaceFrom: 1 to: self size with: self startingAt: 1.	^ newCollection! !!WriteStream methodsFor: 'private' stamp: 'nice 2/26/2009 11:26'!pastEndPut: anObject	"Grow the collection by doubling the size, but keeping the growth between 20 and 1000000.	Then put <anObject> at the current write position."	collection := collection grownBy: ((collection size max: 20) min: 1000000).	writeLimit := collection size.	collection at: (position := position + 1) put: anObject.	^ anObject! !