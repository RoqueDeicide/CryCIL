CryCIL - CRYENGINE game development on the .NET/Mono platform
	by RoqueDeicide (Based on 'CryMono' by Filip 'i59' Lundgren)

# Description
CryCIL is a middleware module that allows to use .Net-based languages to create games powered by CryEngine.
## Current CryEngine version
CryCIL is currently being developed and tested for CryEngine 3.8.1. Update to 5 is planned in the future.

# History
## Origins
Historically engines from CryEngine family allowed to create games using C++ for low-to-mid level logic with Lua used for high level logic. In the 25th of June 2011, Sam Neirinck aka Ins has decided to change this and initiated project "cemono" which goal was to allow high level logic to be done with .Net langeuages (e.g. C#) utilizing the power of Mono.
## CryMono
The project was going quick for a while until Ins started slowing down after just about 100 commits. The work was then taken over by Filip Lundgren aka i59, who branched the project into CryMono. That one proved to have way more life in it, with i59 spending 2 years developing.
## Hiatus
CryMono updates, however, greatly slowed down during CryEngine 3.5 era with barely any features introduced, and compatibility issues fixed only after quite some time.

Release of CryEngine 3.6 became a turning point in the history of CryMono: it broke compatibility while i59 has moved on to SNOW, leaving the project unusable and with nobody to fix it.
## New Caretaker
I'm C# developer with ~6 years of experience, and using C++ or Lua is not something I would desire. So at the beginning of 2014, which is the time when the hiatus began, I've started slowly expanding the functionality of CryMono.

When EaaS was released, it became very clear, that the small changes wouldn't cut it. So in July 2014 I've forked CryMono repository, and started working at full swing.

I have exposed some parts of CryEngine triangular mesh editing interface for starters, then I ported Evan Wallace's implementation of CSG operations from javascript to C#.

The next milestone would be to remove some of the old, too complicated features, that made the development process slow, and debugging very hard. Achieving that goal proved to be a little too much for me, as my inexperience with C++ and lack of understanding of CryMono's internal mechanisms (mostly thanks to poor docs) caused me to botch CryMono completely, leaving no choice other then restarting the project from scratch.
## CryCIL
After restarting the project and naming it CryCIL I've set out to create "v1" version of it which would be the minimal implementation needed for working with Mono. v1 mostly focuses on just establishing virtual interface with Mono with minimal interaction with CryEngine, which means, that it has a potential to be compatible with almost any other version of CE.
# Installation
Download the release of CryCIL that you want to use (e.g. the latest one). Then download release of CryCIL Base Binaries that is mentioned in the notes for your CryCIL release. Unpack both archives into CryEngine installation directory.
# Documentation & Info
Most of the tutorial-style documentation articles will at the Wiki section of the <a href="https://github.com/RoqueDeicide/CryCIL/">GitHub</a> page.

MSDN-style documentation of the API will most likily be hosted at the different location.
