# Plan N Check
Plan N Check is a binary plug in script for Varian Eclipse. 
Its currently being developed as a research tool for starting a radiation therapy **plan** by automatically creating patient beams, fitting collimators, applying constraints to dicom structures automatically, updating, and iteratively adjusting constraints. 
It then runs a plan **check** to test that all standard dose constraints are met, as well as custom constraints that can be set by the planner. 

You can choose however many iterations of VMAT optimization are desired. All but the last iteration will check constraints after and adjust accordingly. 
