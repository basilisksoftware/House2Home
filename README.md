
# House2Home

This is a public version of a project by Jaswinder Singh for Helping Hands Community Project, a homelessness charity based in Leamington Spa, UK. 

From the website:

> Our House2Home project provides furniture and household goods to recently homed & low-income families in South Warwickshire. People are referred to us by organisations like children’s centres, social services & domestic violence charities.

More information on the House2Home project is available [here.](https://www.helpinghandscharity.org.uk/House2Home/)

## Overview
This project is a simple application with an ASPNETCore WebApi backend and an Angular frontend. The application enables the public to get rid of their unwanted items by offering to donate them to the project (usually furniture or electrical items). The offers go to an administration team which can review and process the donation according to their needs, along a predefined workflow (below).

Though this public version is very much in its infancy, it demonstrates a decent level of backend security, including:

 - Authorisation of API requests 
 - JWT authentication 
 - Hashing and salting of passwords
 - Safe storage of app secrets

The application also leverages a free cloud storage api to host images submitted with the public donation offers. The individual cloning this repository would of course have to provide their own credentials.

In brief the workflow is as follows:

> - a) Donor submits form (at least 1 item and at least 1 picture per item)
> 
> - b) Donor gets acknowledgement email, thanking them and telling them HH will get back to them within 72 hours
> 
> - c) Admin gets notification email telling them that a new donation has been submitted
> 
> - d) Admin logs into the admin panel ( {url}/admin )
> 
> - e) The admin clicks on the 'Manage' tab and sees that there is a new entry in the "New Submissions" table
> 
> - f) The admin clicks on the new entry and is taken to the donation management page where they can see various pieces of information
> submitted by the donor, including details and pictures of each item
> 
>  - g)
> 
> 	- g-1) CHOICE 1: The admin decides that there is nothing in this submission they want to collect. They click the 'REJECT' button
> **[WARNING, DONOR WILL BE EMAILED]**:
> 
> 	- g-1.1) The donor is emailed with a polite "thanks but no thanks" email and the donation is moved to the rejected table in the archive
> section
> 
> 	- g-1.2) END OF WORKFLOW
> 
> 	-- OR --
> 
> 	 - g-2) CHOICE 2: The admin decides they want at least one of the items from the submission. They click the 'ACCEPT' button:
> 
> 	- g-2-1) The submissions moves from the 'New Submissions' table to the 'Accepted Submissions' table. Note - the donor will not get an
> email at this stage.
> 
> 	- g-2-2) The admin can now select which items in this submission they want to collect. At least 1 item must be selected in order to progress
> the submission
> 
> 	- g-2-3) The admin selects a date for collection and clicks the 'Arrange Collection' button **[WARNING, DONOR WILL BE EMAILED]**:
> 
> 	-	g-2-4) The donor receives a 'thank you' email detailing which items HH can accept, that HH will try to collect on the date selected, and
> will contact them on the phone number they submitted through the form.
> 
> 	- g-2-5) The submission now moves into the 'Awaiting Collection' table
> 
> 	 - g-2-6) The 'delivery-driver' users can then see the pending collection details on their 'Schedule' tab, and can complete the
> delivery when appropriate by clicking the 'Complete' button, or
> alternatively the admin can also set the submission to 'collected' on
> the driver's behalf.
> 
> 	- g-2-7) When the 'Completed' button is clicked the submission moves into the 'Completed' table
> 
> 	- g-2-8) From here, clicking into the donation will give the admin the option to archive the submission, which will remove all personal
> information from the submission and move it into the archive section
> 
> 	-	g-2-9) **END OF WORKFLOW**
