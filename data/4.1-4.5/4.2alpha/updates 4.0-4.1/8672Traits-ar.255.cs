"Change Set:		8672Traits-ar.255Traits-ar.255:Shipping NanoTraits part 3: A hand ful of small changes that pave the way for the (major) follow-on cleanup."!!ModelExtension class methodsFor: 'initialize-release' stamp: 'ar 12/29/2009 17:05'!initialize	"Unregister subclasses since we're about to go"	self withAllSubclassesDo:[:subclass|		SystemChangeNotifier uniqueInstance noMoreNotificationsFor: subclass.		SystemChangeNotifier uniqueInstance noMoreNotificationsFor: subclass current.	].! !ModelExtension initialize!