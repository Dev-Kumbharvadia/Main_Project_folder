import { Component, inject, OnInit } from '@angular/core';
import { AdminService } from '../../services/admin.service';
import { UserAudit } from '../../model/model';
import { DatePipe } from '@angular/common';

@Component({
  selector: 'app-audit',
  standalone: true,
  imports: [DatePipe],
  templateUrl: './audit.component.html',
  styleUrl: './audit.component.css'
})
export class AuditComponent implements OnInit {
  audits: UserAudit[] = [];
  adminServices = inject(AdminService)

  ngOnInit(): void {
      this.getAllAudits();
  }

  getAllAudits(){
    this.adminServices.getAllAudits().subscribe((res: any)=>{
      this.audits = res;
    });
  }

}
