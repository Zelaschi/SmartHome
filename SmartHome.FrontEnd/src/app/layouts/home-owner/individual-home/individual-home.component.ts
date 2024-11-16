import { Component, Input, OnInit } from '@angular/core';
import HomeCreatedModel from '../../../../backend/services/Home/models/HomeCreatedModel';
import { ActivatedRoute, Router } from '@angular/router';

@Component({
  selector: 'app-individual-home',
  templateUrl: './individual-home.component.html',
  styleUrl: './individual-home.component.css'
})
export class IndividualHomeComponent implements OnInit {
  homeId: string | null = null;
  home: HomeCreatedModel | null = null;
  showMembers: boolean = false;
  showDeviceList: boolean = false;
  isAddingDevice: boolean = false;
  showHomeDevicesList: boolean = false;
  showRoomForm: boolean = false;
  showRooms: boolean = false;
  showHomeNameForm: boolean = false;

  constructor(
    private route: ActivatedRoute,
    private router: Router,
  ) {
    const navigation = this.router.getCurrentNavigation();
    if (navigation?.extras.state) {
      this.home = navigation?.extras.state['homeData'];
    }
  }

  ngOnInit(): void {
    this.homeId = this.route.snapshot.paramMap.get('id');
  }

  GetHomeMembers(): void {
    if (this.homeId) {
      this.showMembers = !this.showMembers;
    } else {
      console.error('ID del hogar no disponible');
    }
  }

  AddDeviceToHome(): void {
    this.showDeviceList = !this.showDeviceList;
    this.isAddingDevice = !this.isAddingDevice;
  }

  onDeviceAdded(): void {
    this.showDeviceList = false;
    this.isAddingDevice = false;
    console.log('Device added succesfully');
  }

  GetHomeDevices(): void {
    this.showHomeDevicesList = !this.showHomeDevicesList;
  }

  CreateRoom(): void {
    this.showRoomForm = !this.showRoomForm;
  }

  GetRooms(): void {
    this.showRooms = !this.showRooms;
  }

  UpdateHomeName(): void {
    this.showHomeNameForm = !this.showHomeNameForm;
  }
  
  onNameUpdated(newName: string): void {
    this.showHomeNameForm = false;
    if (this.home) {
      this.home.name = newName;
    }
    console.log('Home name updated successfully:', newName);
  }
}
