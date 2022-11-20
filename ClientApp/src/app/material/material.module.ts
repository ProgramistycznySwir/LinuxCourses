import {NgModule} from '@angular/core';
import {
  FormsModule,
  ReactiveFormsModule,
} from '@angular/forms';

// import {
//   // MatFormFieldModule,
//   MatButtonModule,
//   MatInputModule,
//   MatDialogModule,
//   MatSidenavModule,
//   MatToolbarModule,
//   MatIconModule,
//   MatListModule,
// } from '@angular/material';
import { MatFormFieldModule } from '@angular/material/form-field'
import { MatButtonModule } from '@angular/material/button'
import { MatInputModule } from '@angular/material/input'
import { MatDialogModule } from '@angular/material/dialog'
// import

@NgModule({
  imports: [
    FormsModule,
    ReactiveFormsModule,
    MatFormFieldModule,
    MatButtonModule,
    MatInputModule,
    MatDialogModule,
    // MatSidenavModule,
    // MatToolbarModule,
    // MatIconModule,
    // MatListModule,
  ],
  exports: [
    FormsModule,
    ReactiveFormsModule,
    MatFormFieldModule,
    MatButtonModule,
    MatInputModule,
    MatDialogModule,
    // MatSidenavModule,
    // MatToolbarModule,
    // MatIconModule,
    // MatListModule,
  ]
})
export class MaterialModule {}