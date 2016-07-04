"Change Set:		8333Collections-ul.236Collections-ul.236:- revert #do: #associationsDo: and the senders of #scanForEmptySlotFor: - removed preamble- add grow tweak to WeakSet- rehash everything at postscript"!!Set methodsFor: 'private' stamp: 'ul 9/20/2009 04:40'!noCheckNoGrowFillFrom: anArray	"Add the elements of anArray except nils to me assuming that I don't contain any of them, they are unique and I have more free space than they require."	1 to: anArray size do: [ :index |		| object |		(object := anArray at: index) ifNotNil: [			array				at: (self scanForEmptySlotFor: object)				put: object ] ]! !!WeakSet methodsFor: 'private' stamp: 'ul 9/20/2009 14:43'!noCheckNoGrowFillFrom: anArray	"Add the elements of anArray except nils and flag to me assuming that I don't contain any of them, they are unique and I have more free space than they require."	tally := 0.	1 to: anArray size do: [ :index |		| object |		((object := anArray at: index) == flag or: [			object == nil ]) ifFalse: [ 				array					at: (self scanForEmptySlotFor: object)					put: object.				tally := tally + 1 ] ]! !!WeakKeyDictionary methodsFor: 'finalization' stamp: 'ul 11/5/2009 04:49'!finalizeValues: finiObjects	"Remove all associations with key == nil and value is in finiObjects.	This method is folded with #rehash for efficiency."		| oldArray |	oldArray := array.	array := Array new: oldArray size.	tally := 0.	1 to: array size do:[ :i |		| association |		(association := oldArray at: i) ifNotNil: [			| key |			((key := association key) == nil and: [ "Don't let the key go away"				finiObjects includes: association value ])					ifFalse: [						array 							at: (self scanForEmptySlotFor: key) 							put: association.						tally := tally + 1 ] ] ]! !!WeakSet methodsFor: 'private' stamp: 'ul 12/1/2009 04:52'!grow	"Grow the elements array if needed.	Since WeakSets just nil their slots, a lot of the occupied (in the eyes of the set) slots are usually empty. Doubling size if unneeded can lead to BAD performance, therefore we see if reassigning the <live> elements to a Set of similiar size leads to a sufficiently (50% used here) empty set first"		| oldTally |	oldTally := tally.	self rehash.	oldTally // 2 < tally ifTrue: [ super grow ]! !!WeakSet methodsFor: 'private' stamp: 'ul 12/1/2009 04:46'!growTo: anInteger	"Grow the elements array and reinsert the old elements"	| oldElements |	oldElements := array.	array := WeakArray new: anInteger.	array atAllPut: flag.	self noCheckNoGrowFillFrom: oldElements! !!Dictionary methodsFor: 'private' stamp: 'ul 9/20/2009 04:40'!noCheckNoGrowFillFrom: anArray	"Add the elements of anArray except nils to me assuming that I don't contain any of them, they are unique and I have more free space than they require."	1 to: anArray size do: [ :index |		| object |		(object := anArray at: index) ifNotNil: [			array				at: (self scanForEmptySlotFor: object key)				put: object ] ]! !!WeakKeyDictionary methodsFor: 'private' stamp: 'ul 9/20/2009 04:40'!noCheckNoGrowFillFrom: anArray	"Add the elements of anArray except nils and flag to me assuming that I don't contain any of them, they are unique and I have more free space than they require."	tally := 0.	1 to: anArray size do:[ :i |		| association |		(association := anArray at: i) ifNotNil: [			array				at: (self scanForEmptySlotFor: association key)				put: association.			tally := tally + 1 ] ]! !!WeakKeyToCollectionDictionary methodsFor: 'as yet unclassified' stamp: 'ul 9/20/2009 04:40'!noCheckNoGrowFillFrom: anArray	"Add the elements of anArray except nils and associations with empty collections (or with only nils) to me assuming that I don't contain any of them, they are unique and I have more free space than they require."	tally := 0.	1 to: anArray size do:[ :i |		| association cleanedValue |		((association := anArray at: i) == nil or: [ 			(cleanedValue := association value copyWithout: nil) isEmpty ]) 				ifFalse: [					association value: cleanedValue.					array						at: (self scanForEmptySlotFor: association key)						put: association.					tally := tally + 1 ] ]! !!KeyedSet methodsFor: 'private' stamp: 'ul 9/20/2009 04:40'!noCheckNoGrowFillFrom: anArray	"Add the elements of anArray except nils to me assuming that I don't contain any of them, they are unique and I have more free space than they require."	1 to: anArray size do: [ :index |		| object |		(object := anArray at: index) ifNotNil: [			array				at: (self scanForEmptySlotFor: (keyBlock value: object))				put: object ] ]! !!Dictionary methodsFor: 'enumerating' stamp: 'ul 11/20/2009 17:21'!associationsDo: aBlock 	"Evaluate aBlock for each of the receiver's elements (key/value 	associations)."	tally = 0 ifTrue: [ ^self].	1 to: array size do: [ :index |		| each |		(each := array at: index)			ifNotNil: [ aBlock value: each ] ]! !!Set methodsFor: 'enumerating' stamp: 'ul 11/20/2009 17:32'!do: aBlock 	tally = 0 ifTrue: [ ^self ].	1 to: array size do: [ :index |		| each |		(each := array at: index)			ifNotNil: [ aBlock value: each ] ]! !!WeakSet methodsFor: 'public' stamp: 'ul 12/1/2009 04:45'!do: aBlock	tally = 0 ifTrue: [ ^self ].	1 to: array size do: [ :index |		| each |		((each := array at: index) == nil or: [ each == flag ])			ifFalse: [ aBlock value: each ] ]! !