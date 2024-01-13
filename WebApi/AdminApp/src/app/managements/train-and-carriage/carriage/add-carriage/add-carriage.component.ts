import {Component, OnInit, ViewChild} from '@angular/core';
import {Observable, of} from 'rxjs';
import {map} from 'rxjs/operators';
import {TrainService} from '../../train/train.service';
import {Train} from '../../../../@models/train';

@Component({
  selector: 'ngx-add-carriage',
  templateUrl: './add-carriage.component.html',
  styleUrls: ['./add-carriage.component.scss'],
})
export class AddCarriageComponent implements OnInit {

  options: Train[];
  filteredOptions$: Observable<Train[]>;

  selectedTrainId: number | null = null;
  isValidTrainSelected: boolean = false;
  hasInteracted: boolean = false;

  @ViewChild('autoInput') input: { nativeElement: { value: string; }; };

  constructor(private trainService: TrainService) {
  }

  ngOnInit() {
    this.trainService.getAllTrainNoPaging().subscribe(trains => {
      this.options = trains;
      this.filteredOptions$ = of(this.options);
    });
  }
  private filter(value: string): Train[] {
    const filterValue = value.toLowerCase();
    return this.options.filter(option => option.name.toLowerCase().includes(filterValue));
  }

  getFilteredOptions(value: string): Observable<Train[]> {
    return of(value).pipe(
      map(filterString => this.filter(filterString)),
    );
  }

  onChange() {
    this.filteredOptions$ = this.getFilteredOptions(this.input.nativeElement.value);
    this.hasInteracted = true;
  }

  onSelectionChange(train: Train) {
    this.selectedTrainId = train.id;
    this.input.nativeElement.value = train.name;
    this.isValidTrainSelected = true; // Train hợp lệ được chọn
  }

  onInputBlur() {
    // Kiểm tra xem tên đoàn tàu có tồn tại hay không khi input thay đổi
    const inputValue = this.input.nativeElement.value;
    const trainExists = this.options.some(train => train.name === inputValue);
    this.isValidTrainSelected = trainExists;
    if (!trainExists) {
      this.selectedTrainId = null;
    }
    this.hasInteracted = true;
  }

}
