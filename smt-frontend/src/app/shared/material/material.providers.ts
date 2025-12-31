import { importProvidersFrom } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatTableModule } from '@angular/material/table';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatSnackBarModule } from '@angular/material/snack-bar';

export const MATERIAL_PROVIDERS = importProvidersFrom(
  MatButtonModule,
  MatCardModule,
  MatToolbarModule,
  MatTableModule,
  MatProgressSpinnerModule,
  MatSnackBarModule
);