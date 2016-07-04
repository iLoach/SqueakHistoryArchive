"Change Set:		9676Exceptions-ar.26Exceptions-ar.26:Add KeyNotFound error which is signaled when a key in a collection cannot be found."!Error subclass: #KeyNotFound	instanceVariableNames: 'key'	classVariableNames: ''	poolDictionaries: ''	category: 'Exceptions-Kernel'!!KeyNotFound methodsFor: 'accessing' stamp: 'ar 3/9/2010 22:02'!messageText	"Return a textual description of the exception."	^messageText ifNil:['Key not found: ', key]! !!KeyNotFound methodsFor: 'accessing' stamp: 'ar 11/20/2007 14:49'!key	"The key which wasn't found"	^key! !!KeyNotFound class methodsFor: 'instance creation' stamp: 'ar 11/20/2007 14:49'!key: anObject	^self new key: anObject! !!KeyNotFound methodsFor: 'accessing' stamp: 'ar 11/20/2007 14:49'!key: anObject	"The key which wasn't found"	key := anObject! !!KeyNotFound methodsFor: 'accessing' stamp: 'ar 11/20/2007 14:52'!description	"Return a textual description of the exception."	^(self messageText) ifNil:['Key not found: ', key]! !