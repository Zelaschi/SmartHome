import { Component, OnInit } from '@angular/core';
import HomeCreatedModel from '../../../../backend/services/Home/models/HomeCreatedModel';
import { ActivatedRoute } from '@angular/router';
import { HomeService } from '../../../../backend/services/Home/home.service';

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
  showAddMembers: boolean = false;

  constructor(
    private route: ActivatedRoute,
    private homeService: HomeService,
  ) {}

  ngOnInit(): void {
    this.route.paramMap.subscribe(params => {
      this.homeId = params.get('id');
      if (this.homeId) {
        this.GetHome();
      } else {
        console.error('No se encontró el ID en la ruta');
      }
    });
  }

  hideAllSections(): void {
    this.showMembers = false;
    this.showDeviceList = false;
    this.isAddingDevice = false;
    this.showHomeDevicesList = false;
    this.showRoomForm = false;
    this.showRooms = false;
    this.showHomeNameForm = false;
    this.showAddMembers = false;
  }

  GetHomeMembers(): void {
    if (this.homeId) {
      if (this.showMembers) {
        this.showMembers = false;
      } else {
        this.hideAllSections();
        this.showMembers = true;
      }
    } else {
      console.error('ID del hogar no disponible');
    }
  }

  AddDeviceToHome(): void {
    if (this.showDeviceList) {
      this.showDeviceList = false;
      this.isAddingDevice = false;
    } else {
      this.hideAllSections();
      this.showDeviceList = true;
      this.isAddingDevice = true;
    }
  }

  onDeviceAdded(): void {
    this.showDeviceList = false;
    this.isAddingDevice = false;
  }

  GetHomeDevices(): void {
    if (this.showHomeDevicesList) {
      this.showHomeDevicesList = false;
    } else {
      this.hideAllSections();
      this.showHomeDevicesList = true;
    }
  }

  CreateRoom(): void {
    if (this.showRoomForm) {
      this.showRoomForm = false;
    } else {
      this.hideAllSections();
      this.showRoomForm = true;
    }
  }

  GetRooms(): void {
    if (this.showRooms) {
      this.showRooms = false;
    } else {
      this.hideAllSections();
      this.showRooms = true;
    }
  }

  UpdateHomeName(): void {
    if (this.showHomeNameForm) {
      this.showHomeNameForm = false;
    } else {
      this.hideAllSections();
      this.showHomeNameForm = true;
    }
  }
  
  AddHomeMembers(): void {
    if (this.showAddMembers) {
      this.showAddMembers = false;
    } else {
      this.hideAllSections();
      this.showAddMembers = true;
    }
  }

  onNameUpdated(newName: string): void {
    if (this.home) {
      this.home.name = newName;
    }
  }

  public GetHome(): void {
    if (this.homeId) {
      this.homeService.GetHomeByHomeId(this.homeId).subscribe(home => {
        this.home = home;
      });
    }
  }
}
