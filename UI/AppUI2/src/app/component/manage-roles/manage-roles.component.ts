import { NgFor, NgIf } from '@angular/common';
import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-manage-roles',
  standalone: true,
  imports: [NgFor, NgIf, FormsModule],
  templateUrl: './manage-roles.component.html',
  styleUrl: './manage-roles.component.css'
})
export class ManageRolesComponent {
// Sample data for roles
roles = [
  { name: 'Admin' },
  { name: 'User' },
  { name: 'Manager' },
  { name: 'Editor' }
];

// Flags for opening modals
isEditModalOpen = false;
isDeleteModalOpen = false;

// Data bindings for the edit modal
editRoleName = '';
roleToDeleteIndex: number | null = null;
roleName: string = '';

openEditModal(index: number): void {
  this.editRoleName = this.roles[index].name;  // Set the current role name in the input field
  this.roleToDeleteIndex = index;  // Set the index of the role being edited
  this.isEditModalOpen = true;  // Open the edit modal
}


// Close the Edit Modal
closeEditModal(): void {
  this.isEditModalOpen = false;
}

// Handle form submission for role editing
onEditSubmit(): void {
  if (this.roleToDeleteIndex !== null) {
    // Update the role name in the roles array
    this.roles[this.roleToDeleteIndex].name = this.editRoleName;
    this.closeEditModal();  // Close the edit modal
  }
}


// Open the Delete Confirmation Modal and set the role to be deleted
openDeleteModal(index: number): void {
  this.roleToDeleteIndex = index;  // Set the index of the role to be deleted
  this.isDeleteModalOpen = true;  // Open the delete modal
}

// Close the Delete Confirmation Modal
closeDeleteModal(): void {
  this.isDeleteModalOpen = false;
}

// Handle role deletion after confirmation
onDeleteConfirmed(): void {
  if (this.roleToDeleteIndex !== null) {
    // Delete the role from the roles array
    this.roles.splice(this.roleToDeleteIndex, 1);
    this.closeDeleteModal();  // Close the delete modal
  }
}

// Handle form submission for role creation
onSubmit(): void {
  if (this.roleName) {
    // Add the new role to the roles array
    this.roles.push({ name: this.roleName });
    console.log('Role saved:', this.roleName);

    // Reset the roleName field after submission
    this.roleName = '';
  } else {
    console.log('Role name is required.');
  }
}
}
