# November 3rd
## Since last time
- Each member of the team wrote a scenario

## Challenges

## Today
- Meeting with Paolo
- Rethink the vertical slice

### Meeting with Paolo 
- Are moderators a different type of user?
	- It would be a role that can be assigned to a user.
	- It could be something like StackOverflow where a user with a lot of points, are more credible and therefore get access to moderator permissions.
	- Credibility is a nice way of dealing with when a user gets moderator permissions.
	- At the beginning there would be a few payed moderators, so there should be a way of specifically upgrading users to moderators.
- Where in the lifetime of content does the moderator live?
	- We should be reactive to uploaded content, so the gate should not be at the update point, but after uploads.
- What is the processes of moderating content, should there be an approval process?
- Does the site only allow access to students?
	- There should be a free part of the platform to demo the site.
	- The real content though should be behind a paywall, that would be payed by universities. This could though also be regular users.
	- There is no need to link the project to a payment provider yet.
	- There could easily be a business model in the technical slice.
	- Users who upload content can see their own content.
	- The premium track is done by email domains
	- Users who produce content could be upgraded to the premium track.
- Who creates content, is it specific content creators or general users?
	- Submitting content can be done by any REGISTERED users.
	- Submitting content is mostly done by payed content producers.
	- The credibility of a user should be visible, so creators should possibly have a specific role, as well as moderators, prolific commenters etc.
- What does "semi automated" mean
	- The platform prides itself on quality content.
	- There should be easy access to information about the video. So something like information about subtitles, resolution, language etc. Could be something like tags that are very public on the list of content.
	- You could possibly fetch content from other platforms if the content is not copyrighted.
	- What is the actual functional requirement of this. Downloading content from and external link or dowloading resources, or something entirely different?
-  Related: Does the site have it's own content or is the entire site links to different web resources, and therefore just a nicely structured repository of external resources.
	- The original idea was just videos.
	- We could also become a conversation and discussion platform by linking other resources.
	- The platform should be community driven, and therefore not as sanitised as something like Khan academy, though Khan academy is very related to the platform we are trying to build.
- When you say "It would be helpful if it were easy to create local copies of the repository." do you mean "It would be helpful if it were easy to create local copies of **sections** of the repository."
	- Just forget that part.
- Specifically what is meant with "respository"
	- The repository is just the content
- How do we provide the data for the hand in
	- A data set should be provided inside a docker container.
	- The data set could also be mocked

#### Vertical slice comments
- The credibility system would check the box of creativity and would be very interesting
- The system should show being dynamic
- The uploading of a video could be a vertical slice in itself
	- But just pressing a button and having it done would be boring
- What would be interesting to show would be uploading 10 files and now being a premium member and level 1 of the system, in terms of the credibility score. 
- Think ALOT about what we want to focus on.
- Think about the demo, how do you make it exciting

#### Scenario comments
- There should not be an "and" in the name
- Focus on the needs of the users, that should be fulfilled by the system
	- Such as "Find high quality tutorial material of C#"
- Scenario 1:
	- There should not be an "and" in the name

### Rethink vertical slice
- Andreas
	- CRUD is dry, but it's what we've been taught, so it would make sense to focus heavily on it no matter what Paolo is saying.
	- We shouldnt adhere from the vertical slice we have already done.
	- Fuck the presentation.
- Joachim
	- CRUD is just the minimum, to get creativity points we have to adhere  from the vertical slice.
	- Would like to work with the credibility
- Adrian
	- We should not work just with uploading and managing materials.
	- We should add something more than just CRUD operations to our vertical slice.
- Amalie
	- Comments and ratings IS expanding on the default CRUD operations on the material.
	- Having a point system would be a vertical slice in itself. This would be really small though.
- Albert
- Asger
	- Could we make a wireframe of the presentation and show that to him, to ensure the product we are about to make is what he actually wants.

#### Variations of the vertical slice
- Remove comments from the original
- ONLY the credibility system
- Complete 180 into focusing on a search algorithm
- Mix of moderation system and credibility system

- Uploading videos (Easy implementation)
- Comments to videos
- Upvoting of comments
- Upvoting of material
- Credibility system
	- Some amount of points add a moderation feature
- Role management through credibility system
	- Moderation role
- Authentication
	- Register
	- Login

## Until next time
- The 2 groups (Amalie, Asger, Alber) and (Andres, Adrian, Joachim) read what the others wrote.
- Each group member creates the use case they have been assigned by Adrian on discord
