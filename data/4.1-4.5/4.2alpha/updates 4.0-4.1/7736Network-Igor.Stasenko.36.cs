"Change Set:		7736Network-Igor.Stasenko.36Network-Igor.Stasenko.36:MailComposition>>openInMorphicafter building the UI window were sent #openInMVC to it.Changed to #openInWorldNetwork-jcg.35:Use 'foo base64Encoded' instead of '(Base64MimeConverter mimeEncode: foo readStream) contents'."!!HTTPSocket class methodsFor: 'get the page' stamp: 'jcg 9/1/2009 00:36'!httpGet: url args: args user: user passwd: passwd	| authorization |	authorization := (user , ':' , passwd) base64Encoded.	^self 		httpGet: url args: args accept: '*/*' 		request: 'Authorization: Basic ' , authorization , CrLf! !!HTTPSocket class methodsFor: 'get the page' stamp: 'jcg 9/1/2009 00:36'!httpPost: url args: args user: user passwd: passwd	| authorization |	authorization := (user , ':' , passwd) base64Encoded.	^self 		httpPostDocument: url args: args accept: '*/*' 		request: 'Authorization: Basic ' , authorization , CrLf! !!HTTPSocket class methodsFor: 'proxy settings' stamp: 'jcg 9/1/2009 00:35'!proxyUser: userName password: password	"Store  HTTP 1.0 basic authentication credentials	Note: this is an ugly hack that stores your password	in your image.  It's just enought to get you going	if you use a firewall that requires authentication"	| encoded |	encoded := (userName, ':', password) base64Encoded.	HTTPProxyCredentials := 'Proxy-Authorization: Basic ' , encoded, String crlf! !!HTTPSocket class methodsFor: 'get the page' stamp: 'jcg 9/1/2009 00:35'!httpPut: contents to: url user: user passwd: passwd	"Upload the contents of the stream to a file on the server"	| bare serverName specifiedServer port page serverAddr authorization s list header firstData length aStream command |	Socket initializeNetwork. 	"parse url"	bare := (url asLowercase beginsWith: 'http://') 		ifTrue: [url copyFrom: 8 to: url size]		ifFalse: [url].	serverName := bare copyUpTo: $/.	specifiedServer := serverName.	(serverName includes: $:) ifFalse: [ port := self defaultPort ] ifTrue: [		port := (serverName copyFrom: (serverName indexOf: $:) + 1 				to: serverName size) asNumber.		serverName := serverName copyUpTo: $:.	].	page := bare copyFrom: (bare indexOf: $/) to: bare size.	page size = 0 ifTrue: [page := '/'].	(self shouldUseProxy: serverName) ifTrue: [ 		page := 'http://', serverName, ':', port printString, page.		"put back together"		serverName := self httpProxyServer.		port := self httpProxyPort].  	"make the request"		serverAddr := NetNameResolver addressForName: serverName timeout: 20.	serverAddr ifNil: [		^ 'Could not resolve the server named: ', serverName].	authorization := (user , ':' , passwd) base64Encoded.	s := HTTPSocket new.	s connectTo: serverAddr port: port.	s waitForConnectionUntil: self standardDeadline.	Transcript cr; show: url; cr.	command := 		'PUT ', page, ' HTTP/1.0', CrLf, 		self userAgentString, CrLf,		'Host: ', specifiedServer, CrLf, 		'ACCEPT: */*', CrLf,		HTTPProxyCredentials,		'Authorization: Basic ' , authorization , CrLf , 		'Content-length: ', contents size printString, CrLf , CrLf , 		contents.	s sendCommand: command.	"get the header of the reply"	list := s getResponseUpTo: CrLf, CrLf ignoring: (String with: CR).	"list = header, CrLf, CrLf, beginningOfData"	header := list at: 1.	"Transcript show: page; cr; show: argsStream contents; cr; show: header; cr."	firstData := list at: 3.	"dig out some headers"	s header: header.	length := s getHeader: 'content-length'.	length ifNotNil: [ length := length asNumber ].	aStream := s getRestOfBuffer: firstData totalLength: length.	s destroy.	"Always OK to destroy!!"	^ header, aStream contents! !!MailComposition methodsFor: 'interface' stamp: 'Igor.Stasenko 9/4/2009 10:10'!openInMorphic	"open an interface for sending a mail message with the given initial 	text "	| textMorph buttonsList sendButton attachmentButton |	morphicWindow := SystemWindow labelled: 'Mister Postman'.	morphicWindow model: self.	textEditor := textMorph := PluggableTextMorph						on: self						text: #messageText						accept: #messageText:						readSelection: nil						menu: #menuGet:shifted:.	morphicWindow addMorph: textMorph frame: (0 @ 0.1 corner: 1 @ 1).	buttonsList := AlignmentMorph newRow.	sendButton := PluggableButtonMorph				on: self				getState: nil				action: #submit.	sendButton		hResizing: #spaceFill;		vResizing: #spaceFill;		label: 'send message';		setBalloonText: 'Accept any unaccepted edits and add this to the queue of messages to be sent';		onColor: Color white offColor: Color white.	buttonsList addMorphBack: sendButton.		attachmentButton := PluggableButtonMorph				on: self				getState: nil				action: #addAttachment.	attachmentButton		hResizing: #spaceFill;		vResizing: #spaceFill;		label: 'add attachment';		setBalloonText: 'Send a file with the message';		onColor: Color white offColor: Color white.	buttonsList addMorphBack: attachmentButton.		morphicWindow addMorph: buttonsList frame: (0 @ 0 extent: 1 @ 0.1).	morphicWindow openInWorld! !