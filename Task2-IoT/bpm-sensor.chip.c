#include "wokwi-api.h"
#include <stdio.h>
#include <stdlib.h>

#define PIN_OUT 35 // Define the pin index for OUT

typedef struct {
  int bpm;
} chip_state_t;

void chip_init() {
  chip_state_t *chip = malloc(sizeof(chip_state_t));
  chip->bpm = 0;
  printf("Hello from BPM sensor!\n");
}

void chip_tick(void *user_data) {
  chip_state_t *chip = (chip_state_t *)user_data;
  
  // Генерация случайного значения BPM от 40 до 170
  chip->bpm = 40 + rand() % 131; // 131, так как диапазон от 40 до 170 включает 170
  
  // Преобразуем BPM в цифровой сигнал и передаем его по пину
  int digital_bpm = chip->bpm % 2; // Пример: передаем младший бит значения BPM
  pin_write(PIN_OUT, digital_bpm);
  
  printf("Current BPM: %d\n", chip->bpm);
}

void chip_deinit(void *user_data) {
  free(user_data);
}

