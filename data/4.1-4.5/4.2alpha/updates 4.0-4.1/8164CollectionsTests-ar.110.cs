"Change Set:		8164CollectionsTests-ar.110CollectionsTests-ar.110:Illustrate a bug in handling collisions with nil keys in Dictionary."!!DictionaryTest methodsFor: 'tests' stamp: 'ar 11/12/2009 21:58'!testNilHashCollision	"Ensures that fixCollisionsFrom: does the right thing in the presence of a nil key"	| dict key |	dict := Dictionary new.	key := nil hash. "any key with same hash as nil"	dict at: key hash put: 1.	dict at: nil put: 2.	self assert: (dict includesKey: nil).	dict removeKey: key.	self assert: (dict includesKey: nil).! !