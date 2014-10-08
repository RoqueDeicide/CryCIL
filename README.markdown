CryCLR - CRYENGINE game development on the .NET/Mono platform
	by RoqueDeicide (Based on 'CryMono' by Filip 'i59' Lundgren)

# Description
CryCLR is a middleware module that allows to use .Net-based languages to create games powered by CryEngine.
# History
## Origins
Historically engines from CryEngine family allowed to create games using C++ for low-to-mid level logic with Lua used for high level logic. In the 25th of June 2011, Sam Neirinck aka Ins has decided to change this and initiated project "cemono" which goal was to allow high level logic to be done with .Net langeuages (e.g. C#) utilizing the power of Mono.
## CryMono
The project didn't last for very long with Ins abandoning it after just &gt;100 commits. The work was then taken over by Filip Lundgren aka i59, who branch the project into CryMono. That one proved to have way more life in it, with i59 spending 2 years developing.
## Hiatus
Good things are not created in a day, and Filip greatly slowed down the progress. It's hard to say, what happened. The most commonly known reason is Herr Lundgren moving to PoppermostProductions, beginning to work on SNOW the game. The author of these lines, however thinks, that the biggest reason was excessive ambitiosness of CryMono: it had way too many features, that were fancy, hard to implement, and some of them bloated the code without even working.
## New Caretaker
Although CryDev community had no care for CryMono, one person, RoqueDeicide (that's me), coudn't accept death of idea. I'm C# developer with ~6 years of experience, and using C++ or Lua is not something I would desire. So at the beginning of 2014, which is the time when the hiatus began, I've started to slowly expand the functionality of CryMono.

When EaaS was released, it became very clear, that is not the small changes that are needed. There are way too many problems, that are also rather problematic to fix. So in July 2014 I've forked CryMono repository, and started working at full swing.

I have exposed some parts of CryEngine triangular mesh editing interface for starters, then I ported Evan Wallace's implementation of CSG operations from javascript to C#. Thinking, what to do next revealed to me, that old features had to go: too much bloat, too much garbage. At that point, branch "SubsystemOverhaul" was created, where I am being a busy bee to the day of writing this.
## CryCLR
The vision of cemono and CryMono was to create means of coding high level logic, low and mid levels were supposed to be handled by C++. That is a vision, different from mine: Mine is to create means of coding Any levels of logic using .Net languages. I have also cut out features like realtime-scripting, and moved the module towards Compile-Once-At-The-Start model. This made me feel that it was appropriate to disassociate my work from CryMono and rebrand it to CryCLR - a project with different goal.
# Documentation & Info
Most of the tutorial-style documentation articles will at the Wiki section of the <a href="https://github.com/RoqueDeicide/CryCLR/">GitHub</a> page.

MSDN-style documentation of the API will most likily be hosted at the different location.
