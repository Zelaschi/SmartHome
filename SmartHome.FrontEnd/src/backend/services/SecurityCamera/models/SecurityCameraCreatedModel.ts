export default interface SecurityCameraCreatedModel {
    id : string,
    type : string,
    modelNumber : string
    description : string,
    photos : string[],
    inDoor : boolean,
    outDoor : boolean,
    movementDetection : boolean,
    personDetection : boolean,
    company : string,
}