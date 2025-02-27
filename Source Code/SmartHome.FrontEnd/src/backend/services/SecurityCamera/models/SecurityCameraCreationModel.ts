export default interface SecurityCameraCreationModel {
    type : string,
    modelNumber : string
    description : string,
    name : string,
    photos : Array<string>,
    inDoor : boolean,
    outDoor : boolean,
    movementDetection : boolean,
    personDetection : boolean,
}