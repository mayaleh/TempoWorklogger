# Importing worklogs Excel file to Tempo

> You havo create the Mapping template fisrt, then you can run the import.

Go to Import section:

![home page](../images/1_import_worklogs.png "homepage")

Select your file:

![File selecting step](../images/2_import_worklogs.png "step1")

After selecting the file, you will se the file info and new button to continue to the next step:

![File info and continue](../images/3_import_worklogs.png "step1Done")

In this step, you will select the mapping template that will be used to read the selected file in the previus step.
Note that if you have only one mapping template, it will be selected by default.

Select the mapping template and check the mapping template settings after selecting it:

![Selecting mapping template](../images/4_import_worklogs.png "step2")

Check the selected mapping tewmplate and click on the new displayd button to go to the next step:

![Checking the selected mapping template](../images/5_import_worklogs.png "step2Done")

Now, you will see the result of the data, that was extracted using the mapping template from the selected file.
You can switch between the table view and the JSON view.
> The json code is representing how the data is sent to the Tempo API

After checking the data, you can confirm and run the import by clicking on the "Execute the import" button:

![Checking loaded data from the file](../images/6_import_worklogs.png "step3")

Import will send the data to the Tempo API one by one and you will see the process results immediately:

> If an error occurred, you will see red row and the reason message with more details.

![Runnig import progress](../images/7_import_worklogs.png "step4")

After the import is completed, the progress bar will hide and message about the completion is displayed:

![Completed import](../images/8_import_worklogs.png "step4Done")