import { Component, inject, OnInit } from '@angular/core';
import { UserInfo } from '../../model/model';
import { AdminService } from '../../services/admin.service';

@Component({
  selector: 'app-manage-user',
  standalone: true,
  imports: [],
  templateUrl: './manage-user.component.html',
  styleUrl: './manage-user.component.css'
})
export class ManageUserComponent implements OnInit {
  users: UserInfo[] = [];
  adminServices = inject(AdminService)

ngOnInit(): void {
  this.getAllUserInfo();
}

getAllUserInfo(){
this.adminServices.getAllUserInfo().subscribe((res:any)=>{
  this.users = res;
  console.log(res);
});
}

banUser(Id: string){

}

}
