"Change Set:		9931Multilingual-ar.118Multilingual-ar.118:Fix drop handling in MultiByteFileStream."!!MultiByteFileStream methodsFor: 'private' stamp: 'ar 4/10/2010 20:48'!requestDropStream: dropIndex	"Needs to install proper converter"	| result |	result := super requestDropStream: dropIndex.	result ifNotNil: [		converter ifNil: [self converter: UTF8TextConverter new].		lineEndConvention ifNil: [ self detectLineEndConvention ]	].	^result! !