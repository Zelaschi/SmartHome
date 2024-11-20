import { Component } from '@angular/core';
import { Subscription } from 'rxjs';
import { UsersService } from '../../../../backend/services/User/users.service';
import UserStatus from './models/user.status';
import userFilters from './models/userFilters';
import { FormControl, FormGroup } from '@angular/forms';

@Component({
  selector: 'app-user-list',
  templateUrl: './user-list.component.html',
  styleUrl: './user-list.component.css'
})
export class UserListComponent {
  private _usersSubscription: Subscription | null = null;
  pageNumber: number = 1;
  pageSize: number = 10;
  loading: boolean = false;

  filters : userFilters = {
    fullName: null,
    role : null,
  }
  constructor(private readonly _usersService: UsersService) {}

  status: UserStatus = {
    moreUsers: true,
    loading: true,
    users: [],
  }

  ngOnDestroy(): void {
    this._usersSubscription?.unsubscribe();
  }

  ngOnInit(): void {
    this.loadUsers();
  }

  loadUsers(): void {
    this.loading = true;
    this._usersService.getAllUsers(this.pageNumber, this.pageSize, this.filters.role, this.filters.fullName).subscribe({
      next: (response) => {
        var moreUsers = response.data.length === this.pageSize;
        if (moreUsers){
          this.status ={
            users: response.data,
            moreUsers: true,
          }
        }else{
          this.status = {
            users: response.data,
            moreUsers: false,
          }
        }
        
        console.log(this.status.users);
        this.loading = false;
      },
      error: (error) => {
        console.error(error);
        this.loading = false;
      }
    });
  }

  onPageChange(page: number): void {
    this.pageNumber = page;
    this.loadUsers();
  }

  readonly formField: any = {
    role: {
      name: 'role',
    },
    fullName: {
      name: 'fullName',
    },
  }

  readonly userFilterForm = new FormGroup({
    [this.formField.role.name]: new FormControl('', []),
    [this.formField.fullName.name]: new FormControl('', []),
  })

  onSubmit(values: any){
    console.log('Datos del user:', values);
    this.status.loading =  true;
    this.filters = {
      role: values.role,
      fullName: values.fullName,
    }
    this.loadUsers();
  }
}
