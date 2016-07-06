'From Squeak3.11alpha of 13 February 2010 [latest update: #9483] on 9 March 2010 at 11:11:23 am'!!HashedCollection class methodsFor: 'initialization' stamp: 'ar 2/26/2010 23:29'!cleanUp: aggressive	"Rehash all instances when cleaning aggressively"	aggressive ifTrue:[self rehashAll].! !!SequenceableCollection methodsFor: 'enumerating' stamp: 'ar 2/27/2010 22:29'!splitBy: aCollection	"Answer the receiver, split by aCollection.	This method works similarly to findTokens: but		a) takes a collection argument (i.e., 'hello<p>world<p>' splitBy: '<p>')		b) is 'strict' in its splitting, for example:				'a///b' findTokens: '/' ==> #('a' 'b')				'a///b' splitBy: '/' ==> #('a' '' '' 'b')	"	^Array streamContents:[:stream|		self splitBy: aCollection do:[:each| stream nextPut: each].	].! !!SequenceableCollection methodsFor: 'enumerating' stamp: 'ar 2/27/2010 22:27'!splitBy: aCollection do: aBlock	"Split the receiver by aCollection. Evaluate aBlock with the parts.	This method works similarly to findTokens: but		a) takes a collection argument (i.e., 'hello<p>world<p>' splitBy: '<p>')		b) is 'strict' in its splitting, for example:				'a///b' findTokens: '/' ==> #('a' 'b')				'a///b' splitBy: '/' ==> #('a' '' '' 'b')	"	| lastIndex nextIndex |	lastIndex := 1.	[nextIndex := self indexOfSubCollection: aCollection startingAt: lastIndex.	nextIndex = 0] whileFalse:[		aBlock value: (self copyFrom: lastIndex to: nextIndex-1).		lastIndex := nextIndex+ aCollection size.	].	aBlock value: (self copyFrom: lastIndex to: self size).! !!Symbol class methodsFor: 'class initialization' stamp: 'ar 2/26/2010 23:06'!cleanUp	"Flush caches"	self compactSymbolTable.! !!WeakRegistry methodsFor: 'adding' stamp: 'ul 2/26/2010 13:49'!add: anObject	"Add anObject to the receiver. Store the object as well as the associated executor."		^self add: anObject executor: anObject executor! !!WeakRegistry methodsFor: 'adding' stamp: 'ul 2/26/2010 14:50'!add: anObject executor: anExecutor	"Add anObject to the receiver. Store the object as well as the associated executor."		self protected: [		(valueDictionary associationAt: anObject ifAbsent: nil)			ifNil: [ valueDictionary at: anObject put: anExecutor ]			ifNotNil: [ :association |				| finalizer |				(finalizer := association value) class == ObjectFinalizerCollection					ifTrue: [ finalizer add: anExecutor ]					ifFalse: [ 						association value: (ObjectFinalizerCollection							with: association value							with: anExecutor) ] ] ].	^anObject! !!WeakRegistry methodsFor: 'copying' stamp: 'ul 2/26/2010 14:54'!postCopy	self protected: [ "Uses the original accessLock"		accessLock := Semaphore forMutualExclusion.		valueDictionary := valueDictionary copy.		valueDictionary associationsDo: [ :each |			each value class == ObjectFinalizerCollection 				ifTrue: [ each value: each value copy ] ].		self installFinalizer ]! !