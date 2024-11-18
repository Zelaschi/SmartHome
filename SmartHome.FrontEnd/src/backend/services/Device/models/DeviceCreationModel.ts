export default interface DeviceCreationModel {
    name: string,
    modelNumber: string,
    description: string,
    photos: Array<string>,
    type: string,
    personDetection: boolean,
    movementDetection: boolean,
    indoor: boolean,
    outdoor: boolean
}