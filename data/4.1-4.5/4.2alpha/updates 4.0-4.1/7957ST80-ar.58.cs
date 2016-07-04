"Change Set:		7957ST80-ar.58ST80-ar.58:Promote ValueHolder and StringHolder to be a Kernel (instead of an ST80) entity. Enables support for Model-based Tools without having MVC loaded."!Object subclass: #Controller	instanceVariableNames: 'model view sensor deferredActionQueue lastActivityTime'	classVariableNames: 'MinActivityLapse'	poolDictionaries: ''	category: 'ST80-Controllers'!MouseMenuController subclass: #ScrollController	instanceVariableNames: 'scrollBar marker savedArea menuBar savedMenuBarArea'	classVariableNames: ''	poolDictionaries: ''	category: 'ST80-Controllers'!Controller subclass: #ScreenController	instanceVariableNames: ''	classVariableNames: 'LastScreenModeSelected'	poolDictionaries: ''	category: 'ST80-Controllers'!Object subclass: #ControlManager	instanceVariableNames: 'scheduledControllers activeController activeControllerProcess screenController newTopClicked'	classVariableNames: ''	poolDictionaries: ''	category: 'ST80-Controllers'!Controller subclass: #MouseMenuController	instanceVariableNames: 'redButtonMenu redButtonMessages'	classVariableNames: ''	poolDictionaries: ''	category: 'ST80-Controllers'!Clipboard class removeSelector: #chooseRecentClipping!StringHolder removeSelector: #selectedClassName!StringHolder removeSelector: #reformulateListNoting:!StringHolder removeSelector: #classCommentIndicated!Smalltalk removeClassNamed: #ValueHolder!StringHolder removeSelector: #buildWith:!StringHolder removeSelector: #perform:orSendTo:!Clipboard class removeSelector: #clipboardText!StringHolder removeSelector: #wantsOptionalButtons!StringHolder class removeSelector: #shiftedYellowButtonMenuItems!Clipboard class removeSelector: #clipboardText:!StringHolder class removeSelector: #codePaneMenu:shifted:!StringHolder removeSelector: #wantsAnnotationPane!StringHolder removeSelector: #annotation!StringHolder class removeSelector: #openLabel:!StringHolder removeSelector: #clearUserEditFlag!Clipboard removeSelector: #primitiveClipboardText:!StringHolder removeSelector: #contents:notifying:!StringHolder class removeSelector: #yellowButtonMenuItems!Clipboard removeSelector: #chooseRecentClipping!StringHolder removeSelector: #noteAcceptanceOfCodeFor:!StringHolder class removeSelector: #initialize!Smalltalk removeClassNamed: #StringHolder!Clipboard removeSelector: #initialize!StringHolder removeSelector: #openSyntaxView!Clipboard class removeSelector: #startUp!StringHolder removeSelector: #reformulateList!StringHolder removeSelector: #selectedClassOrMetaClass!Clipboard removeSelector: #primitiveClipboardText!Clipboard removeSelector: #clearInterpreter!Smalltalk removeClassNamed: #Clipboard!Clipboard removeSelector: #interpreter!StringHolder removeSelector: #contentsSelection!StringHolder removeSelector: #showBytecodes!Clipboard removeSelector: #clipboardText:!StringHolder removeSelector: #codePaneMenu:shifted:!StringHolder removeSelector: #buildOptionalButtonsWith:!StringHolder removeSelector: #textContents:!Clipboard class removeSelector: #default:!StringHolder class removeSelector: #open!StringHolder removeSelector: #openLabel:!StringHolder removeSelector: #defaultContents!StringHolder class removeSelector: #windowColorSpecification!StringHolder removeSelector: #initialize!StringHolder removeSelector: #buildWindowWith:specs:!StringHolder removeSelector: #contents:!ValueHolder removeSelector: #contents:!StringHolder removeSelector: #contents!Clipboard class removeSelector: #default!StringHolder removeSelector: #doItContext!StringHolder removeSelector: #labelString!StringHolder removeSelector: #optionalButtonPairs!ValueHolder removeSelector: #contents!StringHolder removeSelector: #acceptContents:!StringHolder removeSelector: #spawn:!StringHolder removeSelector: #selectedMessageName!StringHolder removeSelector: #doItReceiver!StringHolder removeSelector: #buildCodePaneWith:!Clipboard removeSelector: #clipboardText!StringHolder removeSelector: #buildWindowWith:!Clipboard removeSelector: #setInterpreter!Clipboard class removeSelector: #clearInterpreters!StringHolder removeSelector: #okToChange!Clipboard removeSelector: #noteRecentClipping:!StringHolder initialize!