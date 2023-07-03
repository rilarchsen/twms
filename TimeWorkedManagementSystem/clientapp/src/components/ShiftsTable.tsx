import BrandOne from '../images/brand/brand-01.svg';
import BrandTwo from '../images/brand/brand-02.svg';
import BrandThree from '../images/brand/brand-03.svg';
import BrandFour from '../images/brand/brand-04.svg';
import BrandFive from '../images/brand/brand-05.svg';
import React, {useEffect, useState} from "react";
import {useAPI} from "../hooks/useAPI.tsx";
import {Shift} from "../ApiClient.ts";
import {PencilIcon} from "@heroicons/react/24/outline";
import {useNavigate} from "react-router-dom";

const ShiftsTable = () => {

  const { apiClient } = useAPI();
  const {navigateTo} = useNavigate();

  const [shifts, setShifts] = useState<Shift[]>([]);

  useEffect(() => {
    apiClient.shiftsAll().then((shifts) => {
      setShifts(shifts);
    });
  }, []);

  function msToTime(s) {

    // Pad to 2 or 3 digits, default is 2
    function pad(n, z?) {
      z = z || 2;
      return ('00' + n).slice(-z);
    }

    const ms = s % 1000;
    s = (s - ms) / 1000;
    const secs = s % 60;
    s = (s - secs) / 60;
    const mins = s % 60;
    const hrs = (s - mins) / 60;

    return pad(hrs) + ':' + pad(mins) + ':' + pad(secs);
  }

  function calculateWorkTime(shift: Shift) {
    if (shift.start == null || shift.end == null) return msToTime(0);
    const start = new Date(shift.start);
    const end = new Date(shift.end);
    const diff = end.getTime() - start.getTime();

    if (shift.breaks == null || shift.breaks.length === 0) {
      return msToTime(diff);
    }

    let totalBreakTime = 0;
    for (const b of shift.breaks) {
        if (b.start == null || b.end == null) continue;
        const start = new Date(b.start);
        const end = new Date(b.end);
        const diff = end.getTime() - start.getTime();
        totalBreakTime += diff;
    }

    return msToTime(diff - totalBreakTime);
  }

  const columns = [
    "Company",
    "Date",
    "Start",
    "End",
    "Total length",
    "Breaks",
    "Work time",
    "Actions"
  ];

  return (
    <div className="rounded-sm border border-stroke bg-white px-5 pt-6 pb-2.5 shadow-default dark:border-strokedark dark:bg-boxdark sm:px-7.5 xl:pb-1">
      <h4 className="mb-6 text-xl font-semibold text-black dark:text-white">
        Shifts
      </h4>

      <div className="flex flex-col">
        <div className="grid grid-cols-8 rounded-sm bg-gray-2 dark:bg-meta-4 sm:grid-cols-8">
          {React.Children.toArray(columns.map((column) => (
              <div className="p-2.5 xl:p-5">
                <h5 className="text-sm font-medium uppercase xsm:text-base">
                  {column}
                </h5>
              </div>
          )))}

        </div>
        {/*<div className="grid grid-cols-3 rounded-sm bg-gray-2 dark:bg-meta-4 sm:grid-cols-5">*/}
        {/*  <div className="p-2.5 xl:p-5">*/}
        {/*    <h5 className="text-sm font-medium uppercase xsm:text-base">*/}
        {/*      Source*/}
        {/*    </h5>*/}
        {/*  </div>*/}
        {/*  <div className="p-2.5 text-center xl:p-5">*/}
        {/*    <h5 className="text-sm font-medium uppercase xsm:text-base">*/}
        {/*      Visitors*/}
        {/*    </h5>*/}
        {/*  </div>*/}
        {/*  <div className="p-2.5 text-center xl:p-5">*/}
        {/*    <h5 className="text-sm font-medium uppercase xsm:text-base">*/}
        {/*      Revenues*/}
        {/*    </h5>*/}
        {/*  </div>*/}
        {/*  <div className="hidden p-2.5 text-center sm:block xl:p-5">*/}
        {/*    <h5 className="text-sm font-medium uppercase xsm:text-base">*/}
        {/*      Sales*/}
        {/*    </h5>*/}
        {/*  </div>*/}
        {/*  <div className="hidden p-2.5 text-center sm:block xl:p-5">*/}
        {/*    <h5 className="text-sm font-medium uppercase xsm:text-base">*/}
        {/*      Conversion*/}
        {/*    </h5>*/}
        {/*  </div>*/}
        {/*</div>*/}

        {React.Children.toArray(shifts.map((shift) => (
            <div className="grid grid-cols-8 border-b border-stroke dark:border-strokedark sm:grid-cols-8">
              <div className="flex gap-3 p-2.5 xl:p-5">
                <p className="hidden text-black dark:text-white sm:block">{shift.company?.name || "Company"}</p>
              </div>

              <div className="flex p-2.5 xl:p-5">
                <p className="text-black dark:text-white">{shift.start?.toLocaleDateString()}</p>
              </div>

              <div className="flex p-2.5 xl:p-5">
                <p className="text-black dark:text-white">{shift.start?.toLocaleTimeString()}</p>
              </div>

              <div className="flex p-2.5 xl:p-5">
                <p className="text-black dark:text-white">{shift.end?.toLocaleTimeString()}</p>
              </div>

              <div className="flex p-2.5 xl:p-5">
                <p className="text-black dark:text-white">{
                  shift.start && shift.end && msToTime(shift.end.getTime() - shift.start.getTime())
                }</p>
              </div>

              <div className="flex p-2.5 xl:p-5">
                <p className="text-black dark:text-white">{
                    shift.breaks?.length || 0
                }</p>
              </div>

              <div className="flex p-2.5 xl:p-5">
                <p className="text-black dark:text-white">{
                    calculateWorkTime(shift)
                }</p>
              </div>

              <div className="flex p-2.5 xl:p-5">
                <PencilIcon className={"w-5 h-5 text-black dark:text-white cursor-pointer"} onClick={() => {navigateTo(`/shift/${shift.id}`)}}/>
              </div>
            </div>
        )))}

        {/*<div className="grid grid-cols-8 border-b border-stroke dark:border-strokedark sm:grid-cols-8">*/}
        {/*  <div className="flex items-center gap-3 p-2.5 xl:p-5">*/}
        {/*    <div className="flex-shrink-0">*/}
        {/*      <img src={BrandOne} alt="Brand" />*/}
        {/*    </div>*/}
        {/*    <p className="hidden text-black dark:text-white sm:block">Google</p>*/}
        {/*  </div>*/}

        {/*  <div className="flex items-center justify-center p-2.5 xl:p-5">*/}
        {/*    <p className="text-black dark:text-white">3.5K</p>*/}
        {/*  </div>*/}

        {/*  <div className="flex items-center justify-center p-2.5 xl:p-5">*/}
        {/*    <p className="text-meta-3">$5,768</p>*/}
        {/*  </div>*/}

        {/*  <div className="hidden items-center justify-center p-2.5 sm:flex xl:p-5">*/}
        {/*    <p className="text-black dark:text-white">590</p>*/}
        {/*  </div>*/}

        {/*  <div className="hidden items-center justify-center p-2.5 sm:flex xl:p-5">*/}
        {/*    <p className="text-meta-5">4.8%</p>*/}
        {/*  </div>*/}
        {/*</div>*/}

        {/*<div className="grid grid-cols-3 border-b border-stroke dark:border-strokedark sm:grid-cols-5">*/}
        {/*  <div className="flex items-center gap-3 p-2.5 xl:p-5">*/}
        {/*    <div className="flex-shrink-0">*/}
        {/*      <img src={BrandTwo} alt="Brand" />*/}
        {/*    </div>*/}
        {/*    <p className="hidden text-black dark:text-white sm:block">*/}
        {/*      Twitter*/}
        {/*    </p>*/}
        {/*  </div>*/}

        {/*  <div className="flex items-center justify-center p-2.5 xl:p-5">*/}
        {/*    <p className="text-black dark:text-white">2.2K</p>*/}
        {/*  </div>*/}

        {/*  <div className="flex items-center justify-center p-2.5 xl:p-5">*/}
        {/*    <p className="text-meta-3">$4,635</p>*/}
        {/*  </div>*/}

        {/*  <div className="hidden items-center justify-center p-2.5 sm:flex xl:p-5">*/}
        {/*    <p className="text-black dark:text-white">467</p>*/}
        {/*  </div>*/}

        {/*  <div className="hidden items-center justify-center p-2.5 sm:flex xl:p-5">*/}
        {/*    <p className="text-meta-5">4.3%</p>*/}
        {/*  </div>*/}
        {/*</div>*/}

        {/*<div className="grid grid-cols-3 border-b border-stroke dark:border-strokedark sm:grid-cols-5">*/}
        {/*  <div className="flex items-center gap-3 p-2.5 xl:p-5">*/}
        {/*    <div className="flex-shrink-0">*/}
        {/*      <img src={BrandThree} alt="Brand" />*/}
        {/*    </div>*/}
        {/*    <p className="hidden text-black dark:text-white sm:block">Github</p>*/}
        {/*  </div>*/}

        {/*  <div className="flex items-center justify-center p-2.5 xl:p-5">*/}
        {/*    <p className="text-black dark:text-white">2.1K</p>*/}
        {/*  </div>*/}

        {/*  <div className="flex items-center justify-center p-2.5 xl:p-5">*/}
        {/*    <p className="text-meta-3">$4,290</p>*/}
        {/*  </div>*/}

        {/*  <div className="hidden items-center justify-center p-2.5 sm:flex xl:p-5">*/}
        {/*    <p className="text-black dark:text-white">420</p>*/}
        {/*  </div>*/}

        {/*  <div className="hidden items-center justify-center p-2.5 sm:flex xl:p-5">*/}
        {/*    <p className="text-meta-5">3.7%</p>*/}
        {/*  </div>*/}
        {/*</div>*/}

        {/*<div className="grid grid-cols-3 border-b border-stroke dark:border-strokedark sm:grid-cols-5">*/}
        {/*  <div className="flex items-center gap-3 p-2.5 xl:p-5">*/}
        {/*    <div className="flex-shrink-0">*/}
        {/*      <img src={BrandFour} alt="Brand" />*/}
        {/*    </div>*/}
        {/*    <p className="hidden text-black dark:text-white sm:block">Vimeo</p>*/}
        {/*  </div>*/}

        {/*  <div className="flex items-center justify-center p-2.5 xl:p-5">*/}
        {/*    <p className="text-black dark:text-white">1.5K</p>*/}
        {/*  </div>*/}

        {/*  <div className="flex items-center justify-center p-2.5 xl:p-5">*/}
        {/*    <p className="text-meta-3">$3,580</p>*/}
        {/*  </div>*/}

        {/*  <div className="hidden items-center justify-center p-2.5 sm:flex xl:p-5">*/}
        {/*    <p className="text-black dark:text-white">389</p>*/}
        {/*  </div>*/}

        {/*  <div className="hidden items-center justify-center p-2.5 sm:flex xl:p-5">*/}
        {/*    <p className="text-meta-5">2.5%</p>*/}
        {/*  </div>*/}
        {/*</div>*/}

        {/*<div className="grid grid-cols-3 sm:grid-cols-5">*/}
        {/*  <div className="flex items-center gap-3 p-2.5 xl:p-5">*/}
        {/*    <div className="flex-shrink-0">*/}
        {/*      <img src={BrandFive} alt="Brand" />*/}
        {/*    </div>*/}
        {/*    <p className="hidden text-black dark:text-white sm:block">*/}
        {/*      Facebook*/}
        {/*    </p>*/}
        {/*  </div>*/}

        {/*  <div className="flex items-center justify-center p-2.5 xl:p-5">*/}
        {/*    <p className="text-black dark:text-white">1.2K</p>*/}
        {/*  </div>*/}

        {/*  <div className="flex items-center justify-center p-2.5 xl:p-5">*/}
        {/*    <p className="text-meta-3">$2,740</p>*/}
        {/*  </div>*/}

        {/*  <div className="hidden items-center justify-center p-2.5 sm:flex xl:p-5">*/}
        {/*    <p className="text-black dark:text-white">230</p>*/}
        {/*  </div>*/}

        {/*  <div className="hidden items-center justify-center p-2.5 sm:flex xl:p-5">*/}
        {/*    <p className="text-meta-5">1.9%</p>*/}
        {/*  </div>*/}
        {/*</div>*/}
      </div>
    </div>
  );
};

export default ShiftsTable;
