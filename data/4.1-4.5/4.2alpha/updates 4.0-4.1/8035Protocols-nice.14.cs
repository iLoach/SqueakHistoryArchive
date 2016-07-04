"Change Set:		8035Protocols-nice.14Protocols-nice.14:Use #keys rather than #fasterKeysNote that pattern (x keys asArray sort) could as well be written (x keys sort) now that keys returns an Array...This #asArray is here solely for cross-dialect/fork compatibility."!!Vocabulary methodsFor: 'initialization' stamp: 'nice 10/21/2009 00:17'!strings	| strm |	"Get started making a vocabulary for a foreign language.  That is, build a method like #addGermanVocabulary, but for another language.  	Returns this vocabulary in the same form used as the input used for foreign languages.  To avoid string quote problems, execute	Transcript show: Vocabulary eToyVocabulary strings.and copy the text from the transcript to the method you are building."	"selector		wording			documentation"strm := WriteStream on: (String new: 400).methodInterfaces keys asArray sort do: [:sel |	strm cr; nextPut: $(;		nextPutAll: sel; tab; tab; tab; nextPut: $';		nextPutAll: (methodInterfaces at: sel) wording;		nextPut: $'; tab; tab; tab; nextPut: $';		nextPutAll: (methodInterfaces at: sel) documentation;		nextPut: $'; nextPut: $)].^ strm contents! !