"Change Set:		9814Network-bp.66Network-bp.66:Fix for redirect locations starting with '/', e.g. http://www.squeaksource.com"!!HTTPSocket class methodsFor: 'utilities' stamp: 'bp 3/25/2010 21:32'!ip: byteArrayIP port: portNum urlPath: urlPathString 	^String streamContents: [:stream | 		byteArrayIP			do: [:each | each printOn: stream]			separatedBy: [stream nextPut: $.].		stream nextPut: $:.		portNum printOn: stream.		stream nextPutAll: urlPathString]! !!HTTPSocket class methodsFor: 'utilities' stamp: 'bp 3/25/2010 21:34'!expandUrl: newUrl ip: byteArrayIP port: portNum	(newUrl beginsWith: '../') ifTrue: [^self ip: byteArrayIP port: portNum urlPath: (newUrl allButFirst: 2)].	(newUrl beginsWith: '/') ifTrue: [^self ip: byteArrayIP port: portNum urlPath: newUrl].	^newUrl! !