### 1.2.3 ###
- Update by icebro/bread 🥺
- Fully fixed to work with Seekers of the Storm
- Updated survivor icons for Paladin, Nemesis Commando, Executioner and Chirr
- Added survivor icons for SOTS survivors, Dancer, Johnny, Deputy, Pilot, Ranger, Robomando, Rocket, Sonic, Custodian and Nemesis Mercenary
- Added stage icons for SOTS and Forgotten Relics stages
- Added in-game icon for RiskOfOptions
- Currently Discord game lobbies are still not functional, hopefully fixed in a future update

### 1.2.2 ###
- Updating to work with Seekers of the Storm (thanks to TheBEEGRat for testing)

### 1.2.1 ###
- Added Nemesis Enforcer, Nemesis Commando, Chirr, Executioner, An Arbiter, and Red Mist character icons
- Fix lunar detonation presence
- Add outro (credits) presence
- Fixed an issue where multiplayer would be broken if the user was using EOS
  - This mod's EOS support is still in testing, please let me know if you encounter any game- or mod-breaking issues
  - If you still encounter issues with multiplayer, please make sure you are signed into your Epic Games account if you have enabled crossplay -- this is not a mod issue!
- Fixed an issue where multiplayer clients would have their stage number behind by one
- Fixed an issue where Steam lobbies would update presence incorrectly after a user joins
- Fixed a few behind-the-scenes errors that did not affect gameplay

### 1.2.0 ###
- Added Epic Online Services (EOS) support
  - Ensures lobby presence will work if crossplay is ON
  - May have issues, please let me know if you encounter any!
  - Discord invites and joins may not work while crossplay is enabled
- Added new, higher-resolution images for stages, as well as 7 modded character images (thanks Zan!)
- Fixed display images for Scorched Acres, Abyssal Depths, Void Locus, Planetarium, and Simulacrum
  - Simulacrum runs will now display the image of the current environment being simulated
- Fixed presence not updating from lobby to in-game if the user is a multiplayer client
  - By extension, this streamlines the character selection process for the small image and fixes it entirely on multiplayer clients
- Added a unique presence for the lunar detonation sequence on Commencement
- Unknown/custom characters will now display a question mark and the name of the character (if the character does not have an image in the rich presence database)
- Unknown/custom difficulties will now display the name of the difficulty instead of "Unknown"
- Fixed an issue where bosses would only update on the presence after pausing after the boss is spawned
- Fixed an issue where dying while a boss was alive and starting a new run would not update the presence
- Fixed an issue where exiting to the menu from a multiplayer game would cause the main menu presence to display, rather than the lobby presence
- Fixed an issue where the presence would reset to lobby when another player leaves in a multiplayer game
- Fixed an issue where the user could still receive join requests and send game invites after the run has started

### 1.1.0 ###
- Added Risk of Options support, as a soft dependency
- Added Discord join support (BOTH the host and the person joining need the mod for this to work, this is very finnicky and only works some of the time)
- Updated Teleporter Charge to use enums instead of bytes

### 1.0.1 ###
- (Re)Release