import { Component, Input } from '@angular/core';
import { HomeService } from '../../../../backend/services/Home/home.service';
import UserStatus from './models/UserStatus';
import { User } from '../../../../backend/services/User/models/User';

@Component({
  selector: 'app-home-add-member',
  templateUrl: './home-add-member.component.html',
  styleUrl: './home-add-member.component.css'
})
export class HomeAddMemberComponent {
  @Input() homeId: string | null = null;
  loading: boolean = false;

  constructor(
    private readonly _homeService: HomeService,
  ){}

  status : UserStatus = {
    loading: true,
    users: [],
  }

  ngOnInit(): void {
    if (this.homeId) {
      this.loadUsers(this.homeId);
    }
  }

  onUserAdded(user: User): void {
    if(this.homeId!=null){
      this._homeService.AddHomeMemberToHome(this.homeId, user.id).subscribe({
        next: () => {
          console.log(`Device ${user.name} added to home ${this.homeId} successfully.`);
          if(this.homeId!=null){
            this.loadUsers(this.homeId);
          }
        },
        error: (error: any) => {
          this.status.error = error;
          console.error(`Failed to add user ${user.name} to home:`, error);
        }
      });
    }else{
      console.error("Home Id is null");
    }
  }


  loadUsers(homeId:string):void {
    this.loading = true;
    this._homeService.UnRelatedHomeOwners(homeId).subscribe({
      next: (response) => {
        this.status ={
          users: response,
        }
        console.log(response);
        this.loading = false;
      },
      error: (error) => {
        console.error(error);
        this.loading = false;
      }
    });
  }
}
